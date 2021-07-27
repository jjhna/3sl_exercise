using UnityEngine;

namespace ThreeSpace.Chess
{
    /*
        This is the piece class, it keeps track of the type/class of piece: King, Pawn, Knight...
        By knowing what type the chess piece is we can call the MovmentCheck() function from that class.
        It also keeps track of what color the chess piece is.
    */
    public class Piece : MonoBehaviour
    {
        [SerializeField] public Vector2Int Position;

        //Variables that were added by me
        private bool isWhite;
        private Board board;
        private Tile tile;
        private Piece thisPiece;
        private Transform pieceTransform;
        private bool hasMoved = false;
        private King king;
        private Pawn pawn;
        private Knight knight;


        void Awake() {
            findColor();
            findType();
            board = GetComponentInParent<Board>();
            thisPiece = GetComponent<Piece>();
            pieceTransform = GetComponent<Transform>();
        }

        void Start()
        {
            tile = board.getBoardTile(Position.x, Position.y);
            tile.setHoldsPiece(true);
            tile.setPiece(thisPiece);
        }

        /*
            Function: findType(), Args: none, Returns: none
            A function that uses a string switch statement that tries to find the appropriate type/class 
            that the piece is related to based off it's gameobject name
            For example: if this piece files is attached to a Pawn gameobject then the pawn variable will be initalized
            This is called and used in the Awake function
        */
        private void findType() {
            string _name = this.transform.name;
            switch (_name)
            {
                case "Pawn":
                pawn = new Pawn();
                break;

                case "Knight":
                knight = new Knight();
                break;

                case "King":
                king = new King();
                break;

                default:
                Debug.Log("No match type has been found!");
                break;
            }
        }

        /*
            Function: checkPiecePosition(), Args: the new Tile, Returns boolean true or false: 
            Note: we are checking if the object in question has been initalized, if it has been initalized
            then we will use the following method to move the chess piece
            A function that calls the MovementCheck() from it's appropriate class type (King, pawn...etc)
        */
        public bool checkPiecePosition(Tile _tile) {
            if (pawn != null)
            {
                if (pawn.MovementCheck(Position, isWhite, _tile, board, hasMoved))
                {
                    hasMoved = true;
                    return true;
                } else {
                    return false;
                }
            } else if (knight != null) {
                return knight.MovementCheck(Position, isWhite, _tile);
            } else if (king != null) {
                return king.MovementCheck(Position, isWhite, _tile);
            } else {
                Debug.Log("Something is wrong in the Piece.cs in the checkPiecePosition method, no such Type was found");
                return false;
            }
        }

        //Function: getIsWhite(), Args: none, Returns: boolean isWhite
        //A getter function that checks to see if the color of the chess piece is white or not.
        public bool getIsWhite() {
            return isWhite;
        }

        /*
            Function: setPiecePosition(), Args: the new Vector2Int position and it's (x,y,z) Transform position, 
            Returns: none
            A function that replaces the new tile vector2int coordinates to this chess piece and 
            move this chess piece to the new location using it's transform position
        */
        public void setPiecePosition(Vector2Int newPos, Transform z) {
            Position = newPos;
            pieceTransform.position = z.position;
        }

         /*
            Function: outOfTheGame(), Args: none, Returns: none
            A function that sets this current chess piece to become inactive and temporarily removed from the board.
            Takes no parameters and returns nothing. 
            It also checks if a king piece has been removed from the game, if so then the board script is disabled
            Allowing no further movments while allowing the editor/application to continue to run.
        */
        public void outOfTheGame() {
            if (king != null)
            {
                //game over
                board.enabled = false;
            }
            this.gameObject.SetActive(false);
        }

        /* 
            Function: findColor(), Args: none, Returns: none
            Gets the parent component from the parent transform. 
            Function is to determine if the chess piece is either white or black.
            If the parents compnents name is equivalent to the name "White" 
            then the variable isWhite becomes true, else false. 
        */
        private void findColor() {
            if (transform.parent.name == "White")
            {
                isWhite = true;
            } else {
                isWhite = false;
            }
        }
    }
}
