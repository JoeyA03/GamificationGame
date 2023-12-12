using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridUI : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;

    private GridSystem grid;

    public Sprite imageTile;

    void Start()
    {
        grid = new GridSystem(gridWidth, gridHeight);

        for(int i=0; i<grid.gridArray.GetLength(0); i++)
        {
            for(int j=0; j<grid.gridArray.GetLength(1); j++)
            {
                Debug.Log($"{i}, {j}");
                createGrid(i, j);
            }
        }
    }

    void Update()
    {
        
    }

    void createGrid(int gridWidth, int gridHeight)
    {
        GameObject image = new GameObject();
        image.transform.SetParent(this.transform, true);
        image.AddComponent<RectTransform>();
        image.AddComponent<Image>();

        image.GetComponent<Image>().sprite = imageTile;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);
        image.GetComponent<RectTransform>().pivot = new Vector2(10, 0);

        image.GetComponent<RectTransform>().anchoredPosition = new Vector2(gridWidth * 32, gridHeight * 32);
    }

}
