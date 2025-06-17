using UnityEngine;
using UnityEngine.SceneManagement; 
public class Spike : MonoBehaviour
{
    public string mainMenuSceneName = "Mainmenu"; 
    public GameObject player;
    public GameObject standardrespawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.transform.position = standardrespawn.transform.position;
        }
    }
}
