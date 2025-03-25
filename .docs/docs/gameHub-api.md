---
uid: gameHub-api
title: GameHub API Documentation
---

# GameHub API Documentation

## Overview

The [GameHub](../api/GamePlayService.Services.GameHub.yml) is responsible for managing communication between clients and the server. It handles game creation, game joining, player matching, game state synchronization, moves, and chat messages. It uses SignalR to communicate with clients through the following events.

## Event List

### 1. **GameFound**

**Event Name**: `GameFound`

**Triggered When**: When a game is found and players are added to the game session.

**Parameters**:

- `gameId` (string): The ID of the game that players have joined.

**Client Side Usage**:

```javascript
connection.on("GameFound", function (gameId) {
  console.log("Game found: " + gameId);
});
```

---

### 2. **ReceiveGameState**

**Event Name**: `ReceiveGameState`

**Triggered When**: After a game has started or when the game state is updated (e.g., after a move or a new message).

**Parameters**:

- `gameState` (object): The current state of the game, including the board state, players, messages, and game status.

**Client Side Usage**:

```javascript
connection.on("ReceiveGameState", function (gameState) {
  // Update the game state UI with the new game state
});
```

---

### 3. **ReceiveMove**

**Event Name**: `ReceiveMove`

**Triggered When**: After a player makes a move in the game.

**Parameters**:

- `moveResult ` (object): The result of the move, including success status and message.

**Client Side Usage**:

```javascript
connection.on("ReceiveMove", function (moveResult) {
  // Handle the move result
});
```

---

### 4. **GameFinished**

**Event Name**: `GameFinished`

**Triggered When**: When the game has finished, either due to checkmate, stalemate, or draw.

**Parameters**:

- `looser` (string): The name of the player who lost, or "½-½" in case of a draw.

**Client Side Usage**:

```javascript
connection.on("GameFinished", function (looser) {
  console.log("Game finished. Looser: " + looser);
});
```

---

### 5. **ChatMessageReceived**

**Event Name**: `ChatMessageReceived`

**Triggered When**: When a player sends a chat message in the game.

**Parameters**:

- `chatMessage` (object): The chat message containing the author and text.

**Client Side Usage**:

```javascript
connection.on("ChatMessageReceived", function (chatMessage) {
  // Display the chat message in the game chat UI
});
```

---

## Methods

### 1. **StartGameSearch**

**Method Name**: `StartGameSearch`

**Parameters**:

- `playerName` (string): The name of the player looking for a game.

**Description**: Adds the player to the search queue for finding an opponent.

---

### 2. **StopGameSearch**

**Method Name**: `StopGameSearch`

**Parameters**:

- `playerName` (string): The name of the player who wants to stop searching for a game.

**Description**: Removes the player from the search queue.

---

### 3. **MakeMove**

**Method Name**: `MakeMove`

**Parameters**:

- `gameId` (string): The ID of the game where the move is made.
- `moveDto` (MoveDto): The move made by the player, including the from and to positions, and optional promotion piece.

**Description**: Tries to make a move and notifies all clients with the result and updated game state.

---

### 4. **SendMessage**

**Method Name**: `SendMessage`

**Parameters**:

- `gameId` (string): The ID of the game where the message is sent.
- `chatMessage` (ChatMessageDto): The message to be sent, including the author's name and the message text.

**Description**: Sends a chat message to the game and updates the game state for all players.

---

### 5. **FinishGame**

**Method Name**: `FinishGame`

**Parameters**:

- `gameId` (string): The ID of the game to finish.
- `looser` (string): The name of the player who lost the game, or "½-½" in case of a draw.

**Description**: Ends the game and notifies all players about the result, removing them from the game session.

---

## Conclusion

The GameHub provides several events for real-time communication between the server and client, including game state updates, move results, and chat messages. The API is designed to handle typical game interactions such as starting a game, making moves, sending messages, and determining the outcome of a game.

Only those methods that describe direct interaction with the frontend have been described here. For more information about other methods, please refer to the documentation.
