---
uid: player-system
title: Player System
---

# Player System

The Player System handles the management of players in the chess game. This section explains how players are represented, how they interact with the game, and how the system tracks each player's activity.

## Player Representation

Players are represented as objects with key details such as their name, connection ID (for online play), and time reserve for each game. These details are encapsulated in the [PlayerInGameDto](../api/GamePlay.Dtos.PlayerInGameDto.yml) record.

```csharp
public record PlayerInGameDto(string Name, string ConnectionId, TimeSpan TimeReserve) { }
```

- **Name**: The name of the player (e.g., "Alice" or "Bob").
- **ConnectionId**: A unique identifier for the player's connection, used in multiplayer games to handle communication.
- **TimeReserve**: The amount of time reserved for the player to complete their moves. This reserve decreases as the player makes moves.

## Player Roles

In each game session, players are assigned a role based on their color:

- **White Player**: The player who controls the white pieces and makes the first move.
- **Black Player**: The player who controls the black pieces and moves second.

The [GameSession](../api/GamePlay.Services.GameSessionService.yml) class encapsulates both players:

```csharp
public class GameSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required PlayerInGameDto WhitePlayer { get; set; }
    public required PlayerInGameDto BlackPlayer { get; set; }
    public List<Move> Moves { get; set; } = [];
    public List<ChatMessageDto> Messages { get; set; } = [];
    public string CurrentFen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

In this structure:

- **WhitePlayer** and **BlackPlayer** represent the two participants in the game, encapsulated in the [PlayerInGameDto](../api/GamePlay.Dtos.PlayerInGameDto.yml) records.
- The **GameSession** keeps track of the entire game session, including moves made, messages exchanged, and the current state of the game board.

## Time Management

Each player is given a time reserve, which is tracked separately. This time reserve allows players to make their moves within a set time limit. Once a player's time is up, the game may end in a timeout, depending on the game rules.

> [!WARNING]
> The functionality of the different game modes has not yet been implemented. Currently, the games are played without time control.

## Multiplayer and Chat

In a multiplayer game, players can interact through messages. These chat messages are stored in the `Messages` list of the [GameSession](../api/GamePlay.Services.GameSessionService.yml):

```csharp
public List<ChatMessageDto> Messages { get; set; } = [];
```

Each message contains an author and the text of the message, as represented by the [ChatMessageDto](../api/GamePlay.Dtos.ChatMessageDto.yml) record:

```csharp
public record ChatMessageDto(string Author, string Text) { }
```

This enables real-time communication between players during the game.

For more information on how moves and game sessions are handled, refer to the **[Move System](move-system.md)** and **[GameState and Board](game-state-and-board.md)** sections.

Happy gaming!
