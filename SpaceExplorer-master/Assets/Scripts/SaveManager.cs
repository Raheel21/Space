using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] GameEvent saveEvent;
    [SerializeField] GameEvent loadEvent;

    [SerializeField] GameEvent randomEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        saveEvent.Raise();
    }
    public void Load()
    {
        loadEvent.Raise();
    }

    public void Random()
    {
        randomEvent.Raise();
    }
}
