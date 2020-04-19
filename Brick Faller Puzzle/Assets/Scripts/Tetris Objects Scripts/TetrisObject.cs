using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisObject : MonoBehaviour
{
    float lastFall = 0;
    bool isNextToSpan = false;
    bool downPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (IsValidGridPosition())
            UpdateMatrixGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNextToSpan)
            return;

        if (!IsValidGridPosition())
        {
            //User loose
            UpdateMatrixGrid(); //Must update the grid in another to be able to clearAll and destroy this object.
            FindObjectOfType<GameplayController>().DisplayLoosePanel();
            enabled = false;
            return;
        }

        //If user click on left arrow
        if (GestureManager.ShouldGoLeft() && !downPressed)
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidGridPosition())
            {
                UpdateMatrixGrid();
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }

        //If user click on right arrow
        if (GestureManager.ShouldGoRight() && !downPressed)
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidGridPosition())
            {
                UpdateMatrixGrid();
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }

        //If user click on up arrow
        if (GestureManager.ShouldRotate() && !downPressed)
        {
            transform.Rotate(new Vector3(0, 0, -90));

            if (IsValidGridPosition())
            {
                UpdateMatrixGrid();
                FindObjectOfType<SoundsEffectsController>().PlayRotate();
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }


        //If user click on down arrow OR if he does nothing
        if (GestureManager.ShouldGoDown())
        {
            downPressed = true;
            FindObjectOfType<SoundsEffectsController>().PlayDown();
        }

        if (downPressed || Time.time - lastFall >= 1)
        {
            transform.position += new Vector3(0, -1, 0);

            if (IsValidGridPosition())
            {
                UpdateMatrixGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                int iNbRowDeleted = MatrixGrid.DeleteWholeRows(); //delete all filled rows
                FindObjectOfType<GameplayController>().UpdateScore(iNbRowDeleted); //Add points to total score
                FindObjectOfType<Spawner>().SpawnRandom(); //find gameobject calledSpawner and call one of its function
                enabled = false; //disable current script execution
            }
            lastFall = Time.time;
        }
    }

    bool IsValidGridPosition()
    {
        //transform represent the gameobject attached to that script
        //those gameobjects are the Tetris GameObjects, which are a group of gameobjects. that why transform has children
        //each child represent a block
        foreach (Transform child in transform)
        {
            Vector2 v = MatrixGrid.RoundVector(child.position);

            //if block goes outside the borders.... It s not a valid position
            if (!MatrixGrid.IsInsideBorder(v))
                return false;

            //if block goes on another existing block.... It's not a valid position
            if (MatrixGrid.grid[(int)v.x, (int)v.y] != null && MatrixGrid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }

        return true;
    }

    public void UpdateMatrixGrid()
    {
        //Parse the entire block in the matrix and remove all block that are child of transform (gameObject attached to that script)
        for (int y = 0; y < MatrixGrid.column; ++y)
        {
            for (int x = 0; x < MatrixGrid.row; ++x)
            {
                if (MatrixGrid.grid[x, y] != null)
                {
                    if (MatrixGrid.grid[x, y].parent == transform)
                    {
                        MatrixGrid.grid[x, y] = null;
                    }
                }
            }
        }

        //For all child of transform (gameObject attached to that script)
        //round vector's coordinate and add block into the matrix
        foreach (Transform child in transform)
        {
            Vector2 v = MatrixGrid.RoundVector(child.position);

            //In case this piece is the piece that make user loose : the objet pops on another object. We have to destroy older object in order to be able to ClearAll and restart the game.
            if (MatrixGrid.grid[(int)v.x, (int)v.y] != null)
                Destroy(MatrixGrid.grid[(int)v.x, (int)v.y].gameObject);

            MatrixGrid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public void SetIsNextToSpan(bool b)
    {
        isNextToSpan = b;
    }
}
