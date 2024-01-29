using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public Sprite imageTiles;
    public GridSystem<GridObject> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GridSystem<GridObject>(6, 6, 32, imageTiles, this.transform, (GridSystem<GridObject> g, int x, int y, GameObject go) => new GridObject(g, x, y, go));
        //grid = new GridSystem<GridObject>(6, 6, 32, imageTile, this.transform, () => new GridObject());



        //Debug.Log(grid.gridArray[2, 1] + " FUCK");
        //Debug.Log(grid.gridArray[1, 1] + " FUCK");

        //grid.SetValue(1, 1);
        //Debug.Log(grid.gridArray[1, 1] + " FUCK");
    }

    public class GridObject
    {
        private GridSystem<GridObject> grid;
        private int x;
        private int y;
        private GameObject g;

        public GridObject(GridSystem<GridObject> grid, int x, int y, GameObject g) 
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.g = g;

            this.g = new GameObject("help");
        }

        public override string ToString()
        {
            return $"{x}, {y}";
        }

        public void AddImage() 
        {
            g.AddComponent<Image>();
        }

    }




    public void SetValue(int x, int y)
    {
        int xx;
        int yy;
        xx = x;
        yy = y;

        grid.SetValue(x, y);
        Debug.Log("HelloWORLDGIULASd");
    }

    public void SetValueReset(int x, int y)
    {
        int xx;
        int yy;
        xx = x;
        yy = y;

        grid.SetValueReset(x, y);
        Debug.Log("HelloWORLDGIULASd");
    }
}


