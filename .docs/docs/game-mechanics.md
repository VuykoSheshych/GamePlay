---
uid: game-mechanics
title: Game Mechanics
---

# Game Mechanics

In this section, we describe the mechanics of the chess game implemented in the system, covering the rules and logic behind each aspect of the game.

## General Rules

- **Objective**: The primary goal of the game is to checkmate the opponent's king. A player wins by placing the opponent's king in a position where it cannot escape capture.
- **Turns**: Players take turns to make moves, with white always moving first.
- **Piece Movement**: Each piece has a specific way of moving on the board:
  - **Pawns**: Move forward one square but capture diagonally. On their first move, pawns can move two squares.
  - **Knights**: Move in an L-shape: two squares in one direction and then one square perpendicular to that.
  - **Bishops**: Move diagonally any number of squares.
  - **Rooks**: Move horizontally or vertically any number of squares.
  - **Queens**: Combine the movement of both rooks and bishops.
  - **Kings**: Move one square in any direction and are involved in castling.
- **Castling**: A special move where the king and a rook move simultaneously. The king moves two squares toward the rook, and the rook jumps over the king to the square next to it. Castling can only occur if neither the king nor the rook has moved, and if the squares between them are unoccupied.
- **En Passant**: A special pawn capture that occurs when a pawn moves two squares forward from its starting position and lands next to an opponent's pawn. The opponent can capture it as though it had only moved one square.
- **Promotion**: When a pawn reaches the opponent’s back rank, it can be promoted to a queen, rook, bishop, or knight.

## Special Scenarios

- **Stalemate**: If a player is not in check but has no legal moves, the game ends in a stalemate (a draw).
- **Check**: A king is in check if it is under attack by an opponent's piece. The player must move the king or block the attack to resolve the check.

## Game End

A game can end in several ways:

- **Checkmate**: One player wins by checkmating the opponent's king.
- **Stalemate**: The game ends in a draw if the player cannot make a valid move and is not in check.
- **Resignation**: A player may concede defeat at any time during the game.
- **Timeout**: If a player runs out of time (in timed games), they lose the game.

For detailed information about the rules and how moves are processed, refer to the **[Move System](move-system.md)** section.

Enjoy your game!
