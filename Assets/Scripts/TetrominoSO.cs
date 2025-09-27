using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tetromino", menuName = "Tetris/Tetromino", order = 0)]

public class TetrominoSO : ScriptableObject
{
    [Header("Basic Info")]
    public Tetromino type;
    public Tile tile;

    [Header("Shape")]
    public Vector2Int[] cells;

    [Serializable]
    public class WallKickEntry
    {
        public Vector2Int[] kicks;
    }

    [Header("Rotation Wall Kicks")]
    public WallKickEntry[] wallKicks;

    // Converts wallKicks[] into a 2D array
    public Vector2Int[,] GetWallKicks2D()
    {
        if (wallKicks == null || wallKicks.Length == 0)
            return new Vector2Int[0, 0];

        int rows = wallKicks.Length;
        int cols = 0;

        for (int i = 0; i < rows; i++)
        {
            if (wallKicks[i] != null && wallKicks[i].kicks != null)
                cols = Math.Max(cols, wallKicks[i].kicks.Length);
        }

        var arr = new Vector2Int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            var kicks = wallKicks[i]?.kicks;
            for (int j = 0; j < cols; j++)
            {
                arr[i, j] = (kicks != null && j < kicks.Length) ? kicks[j] : Vector2Int.zero;
            }
        }

        return arr;
    }
}
