using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    public Spike spike;
    public GameObject spawnpos;

    void OnTriggerEnter2D(Collider other){
        spike.standardrespawn= spawnpos ;
    } 
}
