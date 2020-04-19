using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixGrid
{
    public static int row = 10;
    public static int column = 22;

    //Transform = position/rotation/scale of an object
    public static Transform[,] grid = new Transform[row, column];

    public static Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 &&
            (int)pos.x < row &&
            (int)pos.y >= 0 &&
            (int)pos.y <= column;
    }

    public static void DeleteRow(int y)
    {
        for  (int x = 0; x < row; ++x)
        {
            //Use Transform[x, y] to get the reference of the gameObject at this position !
            if (grid[x, y] != null && grid[x, y].gameObject != null)
                //Remove the object in the 2D Scene
                GameObject.Destroy(grid[x, y].gameObject);

            //Remove the object in the matrix
            grid[x, y] = null;
        }
    }

    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < row; ++x)
        {
            if (grid[x, y] != null)
            {
                //Move the object in the matrix
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //Move the object in 2D Scene
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < column; ++i)
        {
            DecreaseRow(i);
        }
    }

    public static bool IsRowFull(int y)
    {
        //In the matrix, parse all column of this row and if one is null, then the row is not full
        for (int x = 0; x < row; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }

        return true;
    }

    public static int DeleteWholeRows()
    {
        int iRowDeleted = 0;

        for (int y = 0; y < column; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
                iRowDeleted++;
            }
        }
        return iRowDeleted;
    }

    public static void ClearAll()
    {
        for (int y = 0; y < column; ++y)
            DeleteRow(y);
    }
}
