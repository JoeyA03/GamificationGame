using UnityEngine;

public class GridSystem 
{
    private int width;
    private int height;
    public int[,] gridArray;

    public GridSystem(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];

        for(int i=0; i<gridArray.GetLength(0); i++)
        {
            for(int j=0; j<gridArray.GetLength(1); j++)
            {
                // Debug.Log($"{i}, {j}");
            }
        }
    }
}
