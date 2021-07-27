using UnityEngine;

namespace ThreeSpace.Chess
{
    /*
        This is the tile class, it contains the coordinates of a tile piece on the board.
        This tile class contains the information if a chess piece is on this tile 
        and the location of this tile so that another chess piece can move to the tile position.
    */
    public class Tile : MonoBehaviour
    {
        [SerializeField] public Vector2Int Position;
        
        //Variables that were added by me
        private bool holdsPiece = false;
        private Piece piece;
        private Transform tileTransform;

        void Awake() {
            tileTransform = GetComponent<Transform>();
        }

        //Function: getHoldsPiece(), Args: none, Returns: boolean holdsPiece
        //A getter function for to see if the tile currently holds a chess piece
        public bool getHoldsPiece() {
            return holdsPiece;
        }

        //Function: setHoldsPieceTrue(), Args: boolean _holdspiece, Returns: none
        //A setter function that sets the the boolean value holdsPiece if the tile does or doesn't hold a chess piece
        public void setHoldsPiece(bool _holdspiece) {
            holdsPiece = _holdspiece;
        }

        //Function: setPiece(), Args: Piece _piece, Returns: none
        //A setter function that replaces the new chess piece that was captured on this tile
        public void setPiece(Piece _piece) {
            piece = _piece;
        }

        //Function: getPiece(), Args: none, Returns: Piece piece
        //A getter function that returns the chess piece that is currently on this tile
        public Piece getPiece() {
            return piece;
        }

        /*  
            Function: getTilePosition(), Args: none, Returns: Transform tileTransform
            A getter function that returns the (x,y,z) position of this tile so that the new chess piece can 
            teleport to this tile location/position.
        */
        public Transform getTilePosition() {
            return tileTransform;
        }
        
    }
}
