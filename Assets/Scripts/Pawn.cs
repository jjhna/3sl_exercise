using UnityEngine;
using ThreeSpace.Chess;

/*
    Note: that this class could have as well been a struct, however since the class contains a private method,
    although not necessary, it keeps up with abstraction, where the user and others don't need to know how it works. 
    Also the class makes it easy in my Piece class to use King king == null to be checked since null checks won't with
    value types (aka structs).

    This class is using the ThreeSPace.Chess class/methods and the ImovementPawn interface
*/
public class Pawn : ImovementPawn
{
    /*
        Function: MovementCheck(), Args: the previous position, the color, the new tile, 
        board class and if the chess piece has preivously moved before , Returns: true or false
        This function performs a few checks before moving the chess piece:
        1. Check to see if the new tile contains a chess piece
        2a. If it doesn't we can check to see if we can move to that new tile location
        2b. If there is a chess piece then the pawn can only attack diagonally
        3. We need to determine if the chess piece is of an opposing color so we can capture that chess piece. 
    */
    public bool MovementCheck(Vector2Int _prevPos, bool _isWhite, Tile _tile, Board _board, bool _hasMoved) {
        Vector2Int newPos = new Vector2Int(_tile.Position.x, _tile.Position.y);

        // see if the tile you're pointing at, currently has a piece on it
        if(_tile.getHoldsPiece()) {
            //if there is a piece then we should assume we can only go diagonally
            if (positionAttack(_prevPos, newPos, _isWhite))
            {
                Piece tilepiece = _tile.getPiece();
                //The opposing color is set inactive from the game
                tilepiece.outOfTheGame();
                return true;
            } else {
                return false;
            }
        } else {
            return positionMove(_prevPos, newPos, _board, _isWhite, _hasMoved);
        }
    }

    /*
        Function: positionMove(), Args: Vector2Int's previous position and new position the board class, 
        the chess piece color, and if the chess piece has been previously moved, Returns: true or false
        This function checks to see if the new target position is within the range of the previous chess piece
        position, so for the pawn, we need to see if the new target position is in within 1 or 2 tiles forward.
        Note: since pawns can move forward 2 tiles, we need to check to see if:
        1. the pawn piece hasn't moved previously before
        2. if there are no other chess piece on the 1st tile in front of the pawn piece
    */
    private bool positionMove(Vector2Int pos, Vector2Int newPos, Board _board, bool _isWhite, bool _hasMoved) {
        Vector2Int value;
        if (_isWhite)
        {
            value = Vector2Int.up;
        } else {
            value = Vector2Int.down;
        }

        if (newPos == pos + (value * 2) && !_hasMoved) {
            //ok we check the 1st square but I need to figure out how to check the 1st square tile position
            Vector2Int total = pos + value;
            Tile tilelocation = _board.getBoardTile(total.x, total.y);
            if (tilelocation.getHoldsPiece())
            {
                //if the 1st spot holds a piece then we return false
                return false;
            } else {
                //otherwise we can resume by moving forward
                return true;
            }
        } else if (newPos == pos + value) {
            // if the coordinates match then the player can move the piece
            return true;
        } else {
            return false;
        }
    }

    /*
        Function: positionAttack(), Args: Vector2Int's previous position and new position the board class, 
        the chess piece color, Returns: true or false
        This function checks to see if the new target position is within the range of the previous chess piece
        position, so for the pawn, we need to check if the enemy chess piece is diagonally in front of the pawn piece.
    */
    private bool positionAttack(Vector2Int pos, Vector2Int newPos, bool _isWhite) {
        //Since pawns only move in one direction, the whites can only move up, while the blacks can move down
        //We need to check the color of the chess piece and move accordingly in that particular direction
        Vector2Int value;
        if (_isWhite)
        {
            value = Vector2Int.up;
        } else {
            value = Vector2Int.down;
        }

        //We can only attack diagonally to the front in the left or right direction
        if (newPos == pos + value + Vector2Int.left)
        {
            return true;
        } else if (newPos == pos + value + Vector2Int.right) {
            return true;
        } else {
            return false;
        }
    }
}
