using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominoes;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set;}
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public GameObject goodJobText;
    public bool isGameActive { get; private set; } = false;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2); //the position is set at zero so we need to offset by half the size of the board
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
    }

    private void Start()
    {
        //SpawnPiece();
    }

    public void SpawnPiece()
    {
        if (!isGameActive) return;

        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];  

        this.activePiece.Initialize(this, this.spawnPosition, data);

        if (isValidPosition(this.activePiece, this.spawnPosition)) 
        {
            Set(this.activePiece);
        }
        else
        {
            isGameActive = false; //the game is over 
            GameManager.Instance.GameOver(); //call the game manager
            this.tilemap.ClearAllTiles(); //clear the board
        }
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++) 
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            this.tilemap.SetTile(tilePosition, piece.Data.Tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool isValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for(int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) //using built in RectIn function to check if we are in bounds of board 
            {
                return false; 
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false; 
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int linesCleared = 0;

        while (row < bounds.yMax)
        {
            if (isLineFull(row))
            {
                StartCoroutine(ShowGoodJob());
                LineClear(row);
                linesCleared++;
            }
            else
            {
                row++; //we only increase when line isnt full because if it is everything will shift down and we wil have to check the same line again. 
            }
        }

        if (linesCleared > 0)
        {
            ScoreManager.Instance.AddScore(linesCleared);
        }

    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }

    }

    private bool isLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for(int col =bounds.xMin;  col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position)) 
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator ShowGoodJob()
    {
        goodJobText.SetActive(true);       
        yield return new WaitForSeconds(0.5f); 
        goodJobText.SetActive(false);     
    }

    public void StartBoard()
    {
        isGameActive = true;
        tilemap.ClearAllTiles();

        if (activePiece != null)
        {
            activePiece.gameObject.SetActive(true); 
        }

        SpawnPiece(); 
    }

    public void StopBoard()
    {
        isGameActive = false;
        tilemap.ClearAllTiles();
        HideActivePiece();
    }

    public void HideActivePiece()
    {
        if (activePiece != null)
        {
            activePiece.gameObject.SetActive(false);
        }
    }

}