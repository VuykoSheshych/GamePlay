---
uid: introduction
title: Introduction
---

# Introduction

Welcome to the documentation for the **Chess GamePlay Project**! This project provides a detailed overview of the chess game's architecture and its core systems, such as game mechanics, board state, player system, and move system.

The project follows a modular approach with various microservices for different game-related functionalities. It uses technologies like **ASP.NET Core**, **SignalR**, **Redis**, **PostgreSQL**, and more to ensure a scalable and efficient gaming experience.

This documentation is intended for developers and users who want to understand the system architecture, game mechanics, and how the game operates.

## Key Features

- **Real-Time Multiplayer**: Players can engage in chess matches with real-time updates using SignalR.
- **Game State Management**: The game state is managed efficiently with Redis for caching and PostgreSQL for storing player data and game records.
- **Player System**: Players can join the game, make moves, and interact with each other in a competitive environment.
- **Move Validation**: The system ensures that each move is valid based on the current state of the game and adheres to chess rules.
- **Game Records**: Completed games are saved with all moves and results for historical reference.

Happy exploring!
