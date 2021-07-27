using UnityEngine;

namespace ThreeSpace.Chess
{
    /*
        This is the board class, it keeps track of 'all' the tile locations on the board.
        It also contains the feature for the user to click and move chess piece around the board.
        Additional added functions include changing the players turn, moving chess pieces and ending the game.
    */
    public class Board : MonoBehaviour
    {
        public delegate void TileEventHandler(Tile tile);

        public event TileEventHandler OnMouseDown;
        public event TileEventHandler OnMouseUp;
        public event TileEventHandler OnMouseHover;

        private Tile[,] _tiles;

        private Tile _lastHovered;

        //Variables that were added by me
        private Tile prevtile;
        private bool whiteturn;

        void Awake()
        {
            whiteturn = true; //We need to initialize to the game that it's white turn first 
            _tiles = new Tile[6, 6];

            foreach (var tile in GetComponentsInChildren<Tile>())
            {
                _tiles[tile.Position.x, tile.Position.y] = tile;
            }
        }

        void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var tile = hit.collider.gameObject.GetComponent<Tile>();

                UpdateHoveredTile(tile);

                if (Input.GetMouseButtonDown(0))
                {
                    OnMouseDown?.Invoke(tile);

                    //  Checks to see if the current tile holds a chess piece, if so then the tile components gets
                    //  pass over to the prevtile variable to be used for the GetMouseButtonUp() 
                    if (tile.getHoldsPiece())
                    {
                        prevtile = tile;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    OnMouseUp?.Invoke(tile);

                    /*  Now we need to check some things about moving the chess piece, these requirements are:
                            1. Is the chess piece color aligned to the current turns color?
                            2. Can the chess piece be placed in the selected tile?
                            3. Are there any identical colored chess piece in the selected tile?
                        If all these requirements are met then the chess piece can be moved to the target.
                    */
                    if (prevtile != null)
                    {
                        /*  Check if the color of our chess piece is the current turns color
                            This method chaining is deemed appropriate since I don't want to initalize a variable
                            everytime I declick a chess piece tile. 
                        */
                        if (prevtile.getPiece().getIsWhite() == whiteturn)
                        {
                            //call the checkTile function to check and see if the chess piece can move to
                            //the intended target tile. Afterwards I need to change the turn for the next color.
                            bool check = checkTile(tile, prevtile);
                            if (check)
                            {
                                movePiece(tile, prevtile);
                                changeTurn();
                            }
                            prevtile = null;
                        }
                    }
                }
            }
            else
            {
                UpdateHoveredTile(null);
            }
        }

        /*
            Function: changeTurn(), Args: none, Returns: none
            A function that changes the boolean, so that it becomes the next colors turn.
            This is done by calling this function after a chess piece has been moved. 
        */
        private void changeTurn() {
            if (whiteturn) {
                whiteturn = false;
            } else {
                whiteturn = true;
            }
        }

        /*
            Function: checkTile(), Args: 1)tile you want to move to & 2)tile that the chess piece was previously at, 
            Returns: boolean from the checkPiecePosition()
            This function calls the checkPiecePosition() function from the piece class to see if it's possible
            to move the chess piece to the new tile location. 
            (Piece)checkPiecePosition() => (Pawn,King...)MovementCheck()
        */
        private bool checkTile(Tile _tile, Tile _prevtile) {
            Piece _piece = _prevtile.getPiece();
            return _piece.checkPiecePosition(_tile);
            //return _prevtile.getPiece()
            //  .checkPiecePosition(_tile);
        }

        /*
            Function: movePiece(), Args: 1)tile you want to move to & 2)tile that the chess piece was previously at, 
            Returns: none
            This function calls multiple functions to ensure that:
            1. the chess piece gets moved from the previous position to the new position.
            2. set the previous tiles boolean setHoldsPiece to false, since the chess piece has moved.
            3. Replace the new tile's piece variable reference to the new chess piece
            4. Declare the old tile's piece variable reference to be null (or doesn't contain a chess piece)
            5. set the new tiles bolean setHoldsPiece to true, since the new tile currently contains a chess piece
        */
        private void movePiece(Tile _tile, Tile _prevtile) {
            Piece prevpiece = _prevtile.getPiece();
            Transform tiletransform = _tile.getTilePosition();
            
            //Get the piece that's on the tile and set the position of the pice to the new tile position
            prevpiece.setPiecePosition(_tile.Position, tiletransform);
            _prevtile.setHoldsPiece(false);
            _tile.setPiece(prevpiece);
            _prevtile.setPiece(null);
            _tile.setHoldsPiece(true);
        }

        /*
            Function: , Args: integers x and y, Returns: A Tile Vector2Int 
            A getter function that gets the x and y coordinates from arguments to see if the 
            tile in question actually exists on the board.
            ie. does _tiles[0, 0] exist on the board?
        */
        public Tile getBoardTile(int x, int y) {
            return _tiles[x,y];
        }

        private void UpdateHoveredTile(Tile tile)
        {
            if (_lastHovered != tile)
            {
                OnMouseHover?.Invoke(tile);
                _lastHovered = tile;
            }
        }
    }
}
