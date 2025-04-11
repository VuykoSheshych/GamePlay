---
uid: gameController-api
title: GameRecordController API Documentation
---

# GameRecordController API Documentation

## Overview

The [GameRecordController](../api/GamePlay.Controllers.GameRecordController.yml) is responsible for the client receiving information about completed games using the HTTP protocol.

## Methods

### 1. **GetGameRecordById**

**Method Name**: `GetGameRecordById`

**HTTP Method**: `GET`

**Endpoint**: `/api/games/{gameId}`

**Parameters**:

- `gameId` (Guid): The ID of the game record to retrieve.

**Returns**:

- `IActionResult`:
  - `Ok(game)` if the game record with the specified `gameId` is found.
  - `NotFound()` if no game record is found for the given `gameId`.

**Description**: Retrieves a game record by its unique ID.

---

### 2. **GetAllGameRecords**

**Method Name**: `GetAllGameRecords`

**HTTP Method**: `GET`

**Endpoint**: `/api/games`

**Returns**:

- `IActionResult`:
  - `Ok(games)` if game records are successfully retrieved.
  - `NotFound()` if no game records are found.

**Description**: Retrieves a list of all available game records.
