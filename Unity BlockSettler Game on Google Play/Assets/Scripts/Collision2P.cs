using UnityEngine;
using System.Collections;

public class Collision2P: MonoBehaviour
{
    private GameManager2P gM;

    public AudioSource gameOver;

    void Awake()
    {
        gM = GetComponent<GameManager2P>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.GetComponent<Renderer>().material.name.Equals(GetComponent<Renderer>().material.name))
        {
            GameManager2P A = new GameManager2P();
            A.gameOverSoundCreate();// gM.restart();
        }
    }
}
