using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows;

    [SerializeField] private int columns;

    [SerializeField] private float tileSizeX = 1.0f;

    [SerializeField] private float tileSizeY = 1.0f;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private List<Color> tileColors;

    private void Start()
    {
        GenerateGrid();
    }

    [Button]
    private void GenerateGrid()
    {
        DestroyGrid();
        var rowColour = 0;
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var newTile = Instantiate(tilePrefab, transform);

                newTile.GetComponent<SpriteRenderer>().color = tileColors[rowColour];

                var posX = (j * tileSizeX);
                var posY = i * -tileSizeY;

                newTile.transform.position = new Vector2(posX, posY);
            }

            rowColour++;
            if (rowColour > tileColors.Count - 1)
            {
                rowColour = 0;
            }
        }

        //Realign the Grid to the middle
        var gridW = columns * tileSizeX;
        var gridH = rows * tileSizeY;
        transform.position = new Vector2(-gridW / 2 + tileSizeX / 2, gridH / 2 - tileSizeY / 2);
    }

    private void DestroyGrid()
    {
        transform.position = Vector2.zero;
        for (var i = transform.childCount; i-- > 0;)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}