using Unity.VisualScripting;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject cam;
    private void OnTriggerStay2D(){
        cam.transform.position = transform.position;
    }
}
