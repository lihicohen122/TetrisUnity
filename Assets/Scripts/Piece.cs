
using UnityEngine;

public class Piece : MonoBehaviour //we only have one piece that we are controlling each time (we can re-initialize it) - thats why we chose to work like this
{
    public Board Board {  get; private set; }
    public Vector3Int Position {  get; private set; }
    public TetrominoData Data { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public int RotationIndex { get; private set; }

    public float stepDelay = 1f; //for yarden- this is how we increase the difficulty 
    public float lockDelay = 0.5f;

    private float stepTime; 
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
       this.Board = board;
       this.Position = position;
       this.Data = data;
       this.RotationIndex = 0;
       this.stepTime = Time.time + stepDelay;
       this.lockTime = 0f;

        if (this.Cells == null)
        {
            this.Cells = new Vector3Int[data.Cells.Length];
        }

        for(int i = 0; i < this.Cells.Length; i++)
        {
            this.Cells[i] = (Vector3Int)data.Cells[i];
        }
    }

    private void Update()
    {
        if (Board == null || !Board.isGameActive) return;

        this.Board.Clear(this);

        this.lockTime += Time.deltaTime;

        //rotations
        if (Input.GetKeyDown(KeyCode.X)) //clockwise rotation 
        {
            Rotate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Z)) //counter-clockwise rotation 
        {
            Rotate(-1);
        }

        //movement left/right 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }

        //movement down 
        if (Input.GetKeyDown(KeyCode.DownArrow)) //this is for soft drop 
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space)) //this is for hard drop- will appear at the ghost animation immediately
        {
            HardDrop();
        }

        if( Time.time >= this.stepTime)
        {
            Step(); 
        }

        this.Board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.Board.Set(this);
        this.Board.ClearLines();
        if (this.Board.isGameActive)
        {
            this.Board.SpawnPiece();
        }
    }

    private void HardDrop() //we move down until we cant do so
    {
        while (Move(Vector2Int.down))
        {
            continue; 
        }

        Lock();
    }

    private bool Move( Vector2Int translation)
    {
        Vector3Int newPosition = this.Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.Board.isValidPosition(this, newPosition);

        if (valid) 
        { 
            this.Position = newPosition;
            this.lockTime = 0f; 
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.RotationIndex;
        this.RotationIndex = Wrap(this.RotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction); 

        if (!TestWallKicks(this.RotationIndex, direction))
        {
            this.RotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }

    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.Cells.Length; i++)
        {
            Vector3 cell = this.Cells[i];
            int x, y;
            switch (this.Data.Tetromino)
            {
                case Tetromino.I:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * RotationUtils.RotationMatrix[0] * direction) + (cell.y * RotationUtils.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * RotationUtils.RotationMatrix[2] * direction) + (cell.y * RotationUtils.RotationMatrix[3] * direction));
                    break;
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * RotationUtils.RotationMatrix[0] * direction) + (cell.y * RotationUtils.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * RotationUtils.RotationMatrix[2] * direction) + (cell.y * RotationUtils.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * RotationUtils.RotationMatrix[0] * direction) + (cell.y * RotationUtils.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * RotationUtils.RotationMatrix[2] * direction) + (cell.y * RotationUtils.RotationMatrix[3] * direction));
                    break;
            }

            this.Cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.Data.WallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.Data.WallKicks[wallKickIndex,i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2; 

        if (rotationDirection < 0)
        {
            wallKickIndex--; 
        }

        return Wrap(wallKickIndex, 0, this.Data.WallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if(input < min)
        {
            return max - (min - input) % (max -min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}