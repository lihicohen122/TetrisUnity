using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TetrominoData
{
    public TetrominoSO definition;

    public Tetromino Tetromino => definition.type;
    public Tile Tile => definition.tile;
    public Vector2Int[] Cells => definition.cells;
    public Vector2Int[,] WallKicks => definition.GetWallKicks2D();
}