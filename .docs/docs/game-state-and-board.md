---
uid: game-state-and-board
title: Game State and Board
---

# Game State and Board

In this section, we explain how the game state and board are represented and managed within the system. This includes the structure of the board, piece placement, and how the game state is tracked during a match.

## Board Representation

The board is represented as an 8x8 grid, where each square can contain either a chess piece or be empty. Each piece is represented by a character that defines its type and color:

- **Uppercase letters** (A-Z) represent white pieces.
- **Lowercase letters** (a-z) represent black pieces.
- **Empty squares** are represented by a space or null character.

For example, the starting position of the pieces is represented as:

```text
rnbqkbnr pppppppp ........ ........ ........ ........ PPPPPPPP RNBQKBNR
```

Here:

- `r` = black rook
- `n` = black knight
- `b` = black bishop
- `q` = black queen
- `k` = black king
- `p` = black pawn
- `P` = white pawn
- `R` = white rook
- `N` = white knight
- `B` = white bishop
- `Q` = white queen
- `K` = white king

## Game State Tracking

The game state includes:

- **Active Color**: The color of the player who is currently taking their turn (either "w" for white or "b" for black).
- **Castling Rights**: The availability of castling for both players. This is represented as a combination of `K` (white king), `Q` (white queen), `k` (black king), and `q` (black queen).
- **En Passant**: Indicates whether en passant capture is possible on the next move. This is recorded as a square, such as `e3` or `-` (if no en passant is possible).
- **Halfmove Clock**: A counter that tracks the number of half-moves since the last pawn move or capture. This is used for the fifty-move rule.
- **Fullmove Number**: The total number of full moves that have been made in the game.

For example, the starting position in Forsyth-Edwards Notation (FEN) is:

```text
rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
```

This notation represents:

1. The board setup.
2. The active color (white to move).
3. Castling rights (`KQkq` means both players have castling rights on both sides).
4. En passant target square (`-` means no en passant).
5. The halfmove clock (`0`).
6. The fullmove number (`1`).

## Updating the Board

Each move updates the board's state:

- Pieces are moved from one square to another.
- Special moves like castling, en passant, or pawn promotion are handled specifically.
- The game state is updated accordingly, including the active color, castling rights, en passant, and halfmove clock.

The game continues until a checkmate, stalemate, resignation, or timeout occurs.

For detailed information about the board representation and updating the game state, refer to the **[Game Mechanics](game-mechanics.md)** and **[Move System](move-system.md)** sections.

Enjoy your game!
