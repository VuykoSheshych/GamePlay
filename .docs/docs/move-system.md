---
uid: move-system
title: Move System
---

# Move System

The **Move System** outlines the mechanics and rules governing the movement of chess pieces in the game.

## Move Validation

Each move made by a player is validated based on the following criteria:

- **Player's Piece**: A player can only move their own pieces. If a player attempts to move an opponent’s piece, the move is rejected.
- **Piece-Specific Movement Rules**: Each chess piece follows specific rules for movement:
  - Pawns can move forward one square, but capture diagonally.
  - Knights move in an "L" shape.
  - Bishops move diagonally, and Rooks move vertically or horizontally.
  - Queens combine the movement of both Bishops and Rooks.
  - Kings move one square in any direction.
- **Target Square**: The destination square must either be empty or occupied by an opponent's piece that can be captured.
- **Move Result**: If the move is valid, the board is updated. If the move leads to a check or checkmate, appropriate actions are taken.

## Move Types

- **Normal Move**: A standard move of a piece from one square to another.
- **Capture Move**: A move that involves taking an opponent's piece.
- **En Passant**: A special pawn capture move under specific conditions.
- **Castling**: A special move involving the King and a Rook under specific conditions.
- **Promotion**: When a pawn reaches the last rank, it can be promoted to a Queen, Rook, Bishop, or Knight.

## Move Notation

Each move is recorded using **Standard Algebraic Notation (SAN)**. For example:

- `e5`: A pawn moves from e5.
- `Nf3`: A knight moves to f3.
- `Qxd5`: A queen captures a piece on d5.
- `O-O`: Kingside castling.

## Move Execution

The execution of a move involves:

1. **Identifying the piece**: Determine which piece is being moved and whether the move is valid.
2. **Updating the board state**: After a valid move, the piece is placed on the destination square, and the originating square is cleared.
3. **Handling Special Moves**: Special moves such as castling or en passant are processed with additional logic to ensure they follow the rules.
