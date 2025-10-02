using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LineClearVFX : MonoBehaviour
{
    public Tilemap fxTilemap;   
    public TileBase fxTile;     


    public int flashes = 2;          // how many on/off flashes
    public float flashInterval = 0.10f; // seconds for each on/off

    private Board board;

    void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public void PlayRow(int row)
    {
        if (fxTilemap == null || fxTile == null || board == null) return;
        StartCoroutine(FlashRow(row));
    }

    private IEnumerator FlashRow(int row)
    {
        var bounds = board.Bounds;

        for (int i = 0; i < flashes; i++)
        {
            // paint overlay tiles across the row
            for (int x = bounds.xMin; x < bounds.xMax; x++)
                fxTilemap.SetTile(new Vector3Int(x, row, 0), fxTile);

            yield return new WaitForSeconds(flashInterval);

            // clear overlay tiles
            for (int x = bounds.xMin; x < bounds.xMax; x++)
                fxTilemap.SetTile(new Vector3Int(x, row, 0), null);

            yield return new WaitForSeconds(flashInterval);
        }
    }
}