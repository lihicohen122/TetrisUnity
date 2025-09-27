using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;

    public Tilemap tilemap {  get; private set; } 
    public Vector3Int[] Cells { get; private set; } 
    public Vector3Int Position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.Cells = new Vector3Int[4];
    }

    private void LateUpdate() //gets called only after all updates- this is important so our ghost only updates after main piece
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < this.Cells.Length; i++)
        {
            Vector3Int tilePosition = this.Cells[i] + this.Position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < this.Cells.Length; i++)
        {
            this.Cells[i] = this.trackingPiece.Cells[i];
        }
    }

    private void Drop()
    {
        Vector3Int position = this.trackingPiece.Position;

        int currentRow = position.y;
        int bottom = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingPiece);

        for( int row = currentRow; row >= bottom; row--) //looping from our piece on the board to the bottom
        {
            position.y = row; 

            if(this.board.isValidPosition(this.trackingPiece, position))
            {
                this.Position = position;
            }
            else
            {
                break;
            }
        }

        this.board.Set(this.trackingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < this.Cells.Length; i++)
        {
            Vector3Int tilePosition = this.Cells[i] + this.Position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
