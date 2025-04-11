using GamePlay.Data;
using GamePlay.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

string recordsDbConnection;
string redisConnection;
string frontendUrl;

if (builder.Environment.IsDevelopment())
{
	// У випадку запуску проєкту в середовищі розробки використовуються підключення через localhost
	recordsDbConnection = builder.Configuration.GetConnectionString("PostgresLocalhostConnection")!;
	redisConnection = builder.Configuration.GetConnectionString("Redis")!;
	frontendUrl = builder.Configuration.GetValue<string>("Url:FrontendUrl")!;
	// When running the project in a development environment, connections via localhost are used
}
else
{
	// В інших випадках використовуються змінні середовища
	recordsDbConnection = Environment.GetEnvironmentVariable("GAMEPLAY_DB_CONNECTION")!;
	redisConnection = Environment.GetEnvironmentVariable("GAMEPLAY_REDIS_CONNECTION")!;
	frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")!;
	// In other cases, environment variables are used
}

builder.Services.AddHealthChecks().AddDbContextCheck<GameDbContext>();

// В проєкті використовується PostgreSQL та "ліниве завантаження"
builder.Services.AddDbContext<GameDbContext>(options =>
	options.UseLazyLoadingProxies().UseNpgsql(recordsDbConnection));
// The project uses PostgreSQL and lazy loading

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
	ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGameRecordService, GameRecordService>();
builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddScoped<IGameSearchService, GameSearchService>();

// Для реалізації гри в шахи в реальному часі використовується SignalR
builder.Services.AddSignalR();
// SignalR is used to implement a real-time chess game

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins(frontendUrl)
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	// У випадку запуску в середовищі розробки, міграції в базі даних будуть застосовуватись автоматично  
	using var scope = app.Services.CreateScope();
	var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
	await dbContext.Database.MigrateAsync();
	// When running in a development environment, database migrations will apply automatically
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gameHub");
app.MapHealthChecks("/health");

app.Run();