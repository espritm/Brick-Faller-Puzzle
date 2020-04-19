using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tetrisObjects;

    private GameObject nextToSpawn = null;


    // Start is called before the first frame update
    void Start()
    {
        SpawnRandom();
    }

    //Spawn at the top
    public void SpawnRandom()
    {
        if (nextToSpawn == null)
        {
            //Select a random integer between 0 and tetrisObject size.
            int index = Random.Range(0, tetrisObjects.Length);

            //Instantiate a new gameobject corresponding to the selected tetrisObject.
            //transform.position is corresponding to "this Spawner" position.
            //Quaternion is used to represent rotation in unity
            Instantiate(tetrisObjects[index], transform.position, Quaternion.identity);

            //Next
            SpawnNext();
        }
        else
        {
            //Make the tetris object "enable"
            TetrisObject obj = nextToSpawn.GetComponent(typeof(TetrisObject)) as TetrisObject;
            obj.SetIsNextToSpan(false);

            //Put the next tetris object at the top of the screen
            Instantiate(nextToSpawn, transform.position, Quaternion.identity);
            
            //Destroy next tetris object before creating a new one
            Destroy(nextToSpawn);
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        //Select a random integer between 0 and tetrisObject size.
        int index = Random.Range(0, tetrisObjects.Length);

        //Instantiate a new gameobject corresponding to the selected tetrisObject.
        //transform.position is corresponding to "this Spawner" position.
        //Quaternion is used to represent rotation in unity
        nextToSpawn = Instantiate(tetrisObjects[index], transform.position + new Vector3(7f, -1), Quaternion.identity);

        //make the tetris object not to fall ("disable" it)
        TetrisObject obj = nextToSpawn.GetComponent(typeof(TetrisObject)) as TetrisObject;
        obj.SetIsNextToSpan(true);
    }
}
