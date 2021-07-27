using UnityEngine;
using ThreeSpace.Chess;

/*
    Note: that this class could have as well been a struct, however since the class contains a private method,
    although not necessary, it keeps up with abstraction, where the user and others don't need to know how it works. 
    Also the class makes it easy in my Piece class to use King king == null to be checked since null checks won't with
    value types (aka structs).

    This class is using the ThreeSPace.Chess class/methods and the Imovement interface
*/
public class Knight : Imovement
{
    /*
        Function: MovementCheck(), Args: the previous position, the color of the chess piece and the new tile class, 
        Returns: true or false
        This function performs a few checks before moving the chess piece:
        1. Check to see if the new tile contains a chess piece
        2a. If it doesn't we can check to see if we can move to that new tile location
        2b. If it does have a chess piece, we need to check and see if the new chess piece is the opposing color
        3. If so then we have to check and see if we can move to that target locatio and remove that chess piece from
        the board.
    */
    public bool MovementCheck(Vector2Int _prevPos, bool _isWhite, Tile _tile) {
        Vector2Int newPos = new Vector2Int(_tile.Position.x, _tile.Position.y);
        
        if(_tile.getHoldsPiece()) {
            Piece tilepiece = _tile.getPiece();
            //Check to see if the chess piece on the new tile is a different color
            if(tilepiece.getIsWhite() != _isWhite) {
                //Calls the positionCheck to see if the piece can move to the area, returns a boolean
                if (positionCheck(_prevPos, newPos))
                {
                    //The opposing color is set inactive from the game
                    tilepiece.outOfTheGame();
                    return true;
                } else {
                    return false;
                }
            } else {
                //If the color is the same piece then we want to avoid it
                return false;
            }
        } else {
            return positionCheck(_prevPos, newPos);
        }
    }
    /*
        Function: positionMove(), Args: Vector2Int's previous position and new position, Returns: true or false
        This function checks to see if the new target position is within the range of the previous chess piece
        position, so for the knight, we need to see if the new target position is in an L shape, if so then
        return true otherwise it's in an invalid spot which means we return false.
    */
    private bool positionCheck(Vector2Int pos, Vector2Int newPos) {
        //The knight can only move in 8 directions, in an L shape, they can also jump over any other piece that 
        //are in the way
        if (newPos == pos + Vector2Int.one + Vector2Int.up || 
        newPos == pos + Vector2Int.one + Vector2Int.right || 
        newPos == pos + Vector2Int.down + (Vector2Int.right * 2) || 
        newPos == pos + Vector2Int.right + (Vector2Int.down * 2) || 
        newPos == pos - Vector2Int.one + Vector2Int.down || 
        newPos == pos - Vector2Int.one + Vector2Int.left ||
        newPos == pos + Vector2Int.up + (Vector2Int.left * 2) || 
        newPos == pos + Vector2Int.left + (Vector2Int.up * 2)) 
        {
            return true;
        } else {
            //No such case or movement can be made to the selected area
            return false;
        }
    }
}
