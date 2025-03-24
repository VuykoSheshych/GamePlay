using GamePlayService.Data;
using GamePlayService.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameDbContext>(options =>
	options.UseLazyLoadingProxies().UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));

builder.Services.AddHealthChecks().AddDbContextCheck<GameDbContext>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
	ConnectionMultiplexer.Connect("gameplay-redis"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GameRecordService>();
builder.Services.AddScoped<GameSessionService>();
builder.Services.AddScoped<GameSearchService>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins("https://localhost:7187") // URL Blazor WASM
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
	await dbContext.Database.MigrateAsync();
	//await dbContext.SeedDataAsync();
}

// Middleware
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
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