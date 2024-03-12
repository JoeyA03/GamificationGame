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

    }
}
