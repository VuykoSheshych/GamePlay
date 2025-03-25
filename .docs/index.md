---
_layout: landing
---

# Welcome to the Chess GamePlay Service Documentation

This documentation will guide you through the architecture, features, and how to get started with the project.

## Quick Start

This project is only a conditional backend part that describes the gameplay logic and presents an external API for interaction.

A detailed description of the external API is contained on the pages:

- [GameHub External API](docs/gameHub-api.md) for gameplay.
- [GameRecordController External API](docs/gameController-api.md) for interaction with the saved games database.

## Documentation Overview

- [Introduction](docs/introduction.md): An overview of the project and its purpose.
- [Game Mechanics](docs/game-mechanics.md): Information about the rules of the chess game and how they are implemented.
- [GameState and Board](docs/game-state-and-board.md): Details on how the chessboard state is represented and managed.
- [Player System](docs/player-system.md): A system for managing players in the game and their Elo ratings after the game is over.
- [Move System](docs/move-system.md): Information on how moves are handled, validated, and stored.

## Project Structure

The project is organized into several key components:

- **Main**: ASP.NET Core API that handles the logic of current games and the management of completed games.
- **Database**: PostgreSQL to store data about finished games and separately about moves for deep analysis.
- **Cache**: Redis to store data about users searching for a game and unfinished games.

## Getting Help

If you need further assistance, join the discussion on our [GitHub Issues](https://github.com/VuykoSheshych/GamePlayService/issues).
