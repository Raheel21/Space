using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RandomPlacement : MonoBehaviour
{
    //data used in the setting up of the objects
    int placed = 0;
    public GameObject[] objectsToPlace;
    public int bounds;
    [SerializeField] string typeName;


    //run when the game first starts
    void Start()
    {
        //create a new array then loop through the child objects and add them to that array
        objectsToPlace = new GameObject[transform.childCount];
        for (int i=0;i<transform.childCount;i++)
        {
            objectsToPlace[i] = transform.GetChild(i).gameObject;
        }
        //stop time so player can decide.
        Time.timeScale = 0;       
    }

    SaveGame SavePositions()
    {
        //create a new SaveGame Object to hold the data
        SaveGame save = new SaveGame();
        //loop round all the objects to grab their x,y,z positions, you could extend this easily for rotation
        for (int i = 0; i < objectsToPlace.Length; i++)
        {
            save.objPosX.Add(objectsToPlace[i].transform.position.x);
            save.objPosY.Add(objectsToPlace[i].transform.position.y);
            save.objPosZ.Add(objectsToPlace[i].transform.position.z);
        }
        //return back the save data to wherever this code is called
        return save;
    }

    [ContextMenu("Save")]
    public void SaveGame()
    {
        //Create the SaveGame object using the above
        SaveGame save = SavePositions();
        //create a new string to hold our JSON
        string jsonString = JsonUtility.ToJson(save);
        //this uses the built in file handling to create a text file persistentDataPath means 
        //we always have the same path and are guaranteed a space to save 
        File.WriteAllText(Application.persistentDataPath + "/typeName.save", jsonString);

        Debug.Log("Saving as JSON: " + jsonString + Application.persistentDataPath + "/typeName.save");
    }

    [ContextMenu("Load")]
    public void LoadGame()
    {
       Time.timeScale = 1;
        //check if the save file exists could use this above to prompt
        if (File.Exists(Application.persistentDataPath + "/typeName.save"))
        {
            //find the file and load it into memory
            string jsonString = File.ReadAllText(Application.persistentDataPath + "/typeName.save");
            //use the JSONUtility to convert back from JSON to string
            SaveGame save = JsonUtility.FromJson<SaveGame>(jsonString);

            Debug.Log("Loading as JSON: " + jsonString);
            
            //loop round the objects and put them back into place
            for (int i = 0; i < objectsToPlace.Length; i++)
            {

                Vector3 position = new Vector3(save.objPosX[i], save.objPosY[i], save.objPosZ[i]);               
                objectsToPlace[i].transform.position = position;
            }

            Debug.Log(gameObject.name+" Game Loaded");
        }
    }

    //Randomly generate the location
    public void RandomGeneration()
    {
        Time.timeScale = 1;
        //Temp variables
        Vector3 NewLocation;
        bool isInside;
        GameObject objectToPlace;

        /**
         * Loop round the list of objects and pick an object, then pick a random empty point
         * use TryGetComponent to work out what collider is on it
         * check to make sure the space is empty then move it to that space.
         */
        for (int i = 0; i < objectsToPlace.Length; i++)
        {
            objectToPlace = objectsToPlace[i];
            NewLocation = (Random.insideUnitSphere)*bounds;
            if (objectToPlace.transform.TryGetComponent(out SphereCollider sphereColl))
            {
                isInside = Physics.CheckSphere(NewLocation, sphereColl.radius);
            }
            else if (objectToPlace.transform.TryGetComponent(out MeshCollider meshColl))
            {
                isInside = Physics.CheckSphere(NewLocation, transform.GetComponentInChildren<MeshCollider>().bounds.extents.magnitude);
            }
            else
            {
                isInside = false;
            }



            /**
            while (isInside)
            {
                NewLocation = (Random.insideUnitSphere * bounds);
                isInside = Physics.CheckSphere(NewLocation, objectToPlace.transform.GetComponent<SphereCollider>().radius);
            }*/
            objectToPlace.transform.position = NewLocation;

        }
    }
}
