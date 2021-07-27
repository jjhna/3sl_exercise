using UnityEngine;
using ThreeSpace.Chess;

/*
    Interface IMovement, Args: The vector2Int of the previous position, the current color of the chess piece,
    and the new tile position.

    This interface is built for the King and Knight class to see if both chess pieces can move/attack to a 
    specific tile piece.
*/
public interface Imovement
{
    bool MovementCheck(Vector2Int _prevPos, bool _isWhite, Tile _tile);
}

/*
    Interface ImovementPawn, Args: The vector2Int of the previous position, the current color of the chess piece,
    and the new tile position, the board class and a boolean to see if the chess piece has previously moved before

    This interface is built specifically for the Pawn class to see if it can move or attack at a specific tile piece.

    Note: _board and _hasMoved is needed so that we can ensure that the pawn piece can move forward by 2 tiles:
    1. to use the _board to see if the tile in front of the chess piece contains a chess piece  
    2. to check the boolean _hasMoved to see if the chess piece has previously moved before
*/
public interface ImovementPawn
{
    bool MovementCheck(Vector2Int _prevPos, bool _isWhite, Tile _tile, Board _board, bool _hasMoved);
}