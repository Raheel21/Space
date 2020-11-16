using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffect : MonoBehaviour
{
    public ParticleSystem deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayDeath()
    {
        deathEffect.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        if(gameObject.tag=="Player")
        GameObject.Find("OptionsManager").GetComponent<ScoreManager>().gs = ScoreManager.gameState.enterscore;
    }
}
