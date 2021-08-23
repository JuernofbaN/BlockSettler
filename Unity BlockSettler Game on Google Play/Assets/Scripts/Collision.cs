
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    private GameManager gM;

    public AudioSource gameOver;

    void Awake()
    {
        gM = GetComponent<GameManager>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.GetComponent<Renderer>().material.name.Equals(GetComponent<Renderer>().material.name))
        {
            GameManager A = new GameManager();
            A.gameOverSoundCreate();// gM.restart();
        }
    }

}