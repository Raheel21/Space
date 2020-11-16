using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//must be marked Serializable and not a monobehaviour
[System.Serializable]
public class SaveGame
{
    //the objects positions, we cannot use Vector3s directly as these are complex types
    public List<float> objPosX = new List<float>();
    public List<float> objPosY = new List<float>();
    public List<float> objPosZ = new List<float>();

    //Creates a copy of this class from a given JSON string and returns it
    public static SaveGame CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SaveGame>(jsonString);
    }

}
