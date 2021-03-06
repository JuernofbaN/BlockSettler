using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Advertisements;
//using Random = System.Random;

public class GameManager2P : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    string gId = "3771415";
   // string video = "video";
    private static int adCounter2 = 0;
    GameObject objects;
    private static int turn = 2; // 1 for player 1 , 2 for player 2 default 1;
    public GameObject floor;
    public GameObject Cube;
    public GameObject Sphere;
    public GameObject Capsule;
    public GameObject Kaseler1;
    public GameObject Kaseler2;
    public GameObject Kaseler3;
    public GameObject Kaseler4;
    public GameObject Kaseler5;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject losePanel2;

    public GameObject kasem1;
    public GameObject kasem2;
    public GameObject kasem3;
    public GameObject kasem4;
    public GameObject kasem5;
    public GameObject kasem6;
    public GameObject kasem7;
    public GameObject kasem8;
    public GameObject kasem9;
    public GameObject kasem10;
    public GameObject kasem11;
    public GameObject kasem12;
    public GameObject kasem13;
    public GameObject kasem14;
    public GameObject kasem15;

    private Material sky;
    public Transform fllPosition;

    // SHAKE CAM
    public Transform camTransform;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.4f;
    public float decreaseFactor = 1.0f;
    Vector3 originalPos;
    //

    public Text txtScore;
    public Text levelTxt;
    public Text stageTxt;
    public Text player1Txt;
    public Text player2Txt;

    private int score = 0;
    private static int level = 1;
    private static int stage = 1;
    private static int objectIndex = 0;

    public float rotationMultiplierLeft = 1f;
    public float rotationMultiplierRight = -1f;
    private float sinMultiplier = 2f;
    float period = 2f;
    float mvmntFactor;

    bool isPressedCreate = false;
    bool isThrowed = false;
    bool isActiveLeft = false;
    bool isActiveRight = false;
    static bool isGameOver = false;
    bool collided = false;

    Vector3 movement = new Vector3(2f, 0f, 0f);
    Vector3 startingPos;

    public AudioSource createObjectSound;
    public AudioSource gameFinished;
    public AudioSource buttonSound;
    public AudioSource successSound;

    private void Start()
    {
        Advertisement.Initialize(gId, false);
        WhoseTurnTextHandler();
        isGameOver = false;
        DataHandling();
        startingKase();
        panelsStartActiveFalse();
        mCameraChanger();
        stageTextChanger();
        levelTextChanger();
        objects = Cube;
        startingPos = Cube.GetComponent<Transform>().position;
        startingColorHandler();
        objectCheck();
        createNewObject();
        score = 0;
        startingColorHandler();
    }

    private void Update()
    {
      //  Debug.Log("Advi. ctr = " + adCounter);
        shakeCameraOnLose();
        WhoseTurnTextHandler();
        RotationController();
        if (!isThrowed)
            movementObjectAtoSin();
        if (score == (stage * 3) + (level * 3))
        {
            score = 0;
            stage++;
            if (stage == 6)
            {
                stage = 1;
                level++;
            }
            adCounter2++;
            PlayerPrefs.SetInt("KeyOne", stage);
            PlayerPrefs.SetInt("KeyTwo", level);
            successSoundCreate();
            //loadPlayer.SavePlayer();
        }
        if (isGameOver)
        {
            if (!collided)
            {
                adCounter2++;
                collided = true;
                shakeDuration = 4.0f;
                gameFinished.Play();
                //Debug.Log("Girdim La içeri");
                if (turn == 1)
                {
                    losePanel.SetActive(true);
                }
                else
                {
                    losePanel2.SetActive(true);
                }
                adCounter2++;
                score = 0;
                Invoke("loadingGameScene", 2.5f);
            }
        }
        else
        {
            ColorChangerr();
        }
        if(adCounter2 > 3)
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
                adCounter2 = 0;
            }
        }
    }

    private void Awake()
    {
        if (camTransform == null)
            camTransform = GetComponent(typeof(Transform)) as Transform;
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void shakeCameraOnLose()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + (UnityEngine.Random.insideUnitSphere * shakeAmount);
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.position = originalPos;
        }
    }

    private void WhoseTurnTextHandler()
    {
        if (turn == 1)
        {
            player1Txt.text = "Player 1";
            player2Txt.text = "";
        }
        else
        {
            player2Txt.text = "Player 2";
            player1Txt.text = "";
        }
    }

    private void panelsStartActiveFalse()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void startingKase()
    {
        Kaseler1.SetActive(false);
        Kaseler2.SetActive(false);
        Kaseler3.SetActive(false);
        Kaseler4.SetActive(false);
        Kaseler5.SetActive(false);
        int kaseStage = (stage % 5);
        if (kaseStage == 1)
            Kaseler1.SetActive(true);
        if (kaseStage == 2)
            Kaseler2.SetActive(true);
        if (kaseStage == 3)
            Kaseler3.SetActive(true);
        if (kaseStage == 4)
            Kaseler4.SetActive(true);
        if (kaseStage == 0) // for 5;
            Kaseler5.SetActive(true);
    }

    private void mCameraChanger()
    {
        Material newMat;
        if (stage == 1)
        {
            newMat = Resources.Load("FloorMat1", typeof(Material)) as Material;
            sky = Resources.Load("CloudyCrown_Daybreak", typeof(Material)) as Material;
        }
        else if (stage == 2)
        {
            newMat = Resources.Load("FloorMat2", typeof(Material)) as Material;
            sky = Resources.Load("CloudyCrown_Midday", typeof(Material)) as Material;
        }
        else if (stage == 3)
        {
            newMat = Resources.Load("FloorMat3", typeof(Material)) as Material;
            sky = Resources.Load("CloudyCrown_Evening", typeof(Material)) as Material;
        }
        else if (stage == 4)
        {
            newMat = Resources.Load("FloorMat4", typeof(Material)) as Material;
            sky = Resources.Load("CloudyCrown_Sunset", typeof(Material)) as Material;
        }
        else
        {
            newMat = Resources.Load("FloorMat5", typeof(Material)) as Material;
            sky = Resources.Load("CloudyCrown_Midnight", typeof(Material)) as Material;
        }
        floor.GetComponent<Renderer>().material = newMat;
        RenderSettings.skybox = sky;
    }

    private void objectCheck()
    {
        System.Random rnd = new System.Random();
        int month = rnd.Next(0, 3);

        objectIndex = month;
        if (objectIndex == 0)
        {
            objects = Cube;
        }
        else if (objectIndex == 1)
        {
            objects = Sphere;
        }
        else
        {
            objects = Capsule;
        }
    }

    private void movementObjectAtoSin()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);
        mvmntFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = mvmntFactor * movement * sinMultiplier;
        objects.GetComponent<Transform>().position = startingPos + offset;
    }

    private void RotationController()
    {
        if (isActiveLeft)
        {
            rotateLeft();
        }

        if (isActiveRight)
        {
            rotateRight();
        }
    }

    private void levelTextChanger()
    {
        levelTxt.text = "Level " + level.ToString();
    }

    private void stageTextChanger()
    {
        stageTxt.text = "Stage " + stage.ToString();
    }

    public void onPointerDownLeft()
    {
        isActiveLeft = true;
    }

    public void onPointerUpLeft()
    {
        isActiveLeft = false;
    }

    public void onPointerDownRight()
    {
        isActiveRight = true;
    }

    public void onPointerUpRight()
    {
        isActiveRight = false;

    }

    public void releaseButtonPressed()
    {
        if (isPressedCreate)
        {
            return;
        }
        else
        {
            isPressedCreate = true;
            soundCreate();
            objects.GetComponent<Rigidbody>().useGravity = true;
            Invoke("createNewObject", 2);
            isThrowed = true;
        }
    }

    public void rotateLeft()
    {
        objects.transform.Rotate(0.0f, 0.0f, rotationMultiplierLeft, Space.World);
    }

    public void rotateRight()
    {
        objects.transform.Rotate(0.0f, 0.0f, rotationMultiplierRight, Space.World);
    }

    public void startingColorHandler()
    {
        Material newMat = Resources.Load("Red", typeof(Material)) as Material; // DEFAULT
        System.Random rnd = new System.Random();
        int month = rnd.Next(1, 8);
        if (month == 1)
        {
            newMat = Resources.Load("Red", typeof(Material)) as Material;

        }
        else if (month == 2)
        {
            newMat = Resources.Load("Blue", typeof(Material)) as Material;
        }
        else if (month == 3)
        {
            newMat = Resources.Load("Pink", typeof(Material)) as Material;
        }
        else if (month == 4)
        {

            newMat = Resources.Load("Green", typeof(Material)) as Material;
        }
        else if (month == 5)
        {
            newMat = Resources.Load("NavyBlue", typeof(Material)) as Material;
        }
        else if (month == 6)
        {
            newMat = Resources.Load("Yellow", typeof(Material)) as Material;
        }
        else
        {
            newMat = Resources.Load("Orange", typeof(Material)) as Material;
        }
        objects.GetComponent<Renderer>().material = newMat;
    }

    public void createNewObject()
    {
        objectCheck();
        //Debug.Log("Stage = " + stage);
        GameObject go = Instantiate(objects, fllPosition.position, Quaternion.identity);
        objects = go;
        go.SetActive(true);
        objects.GetComponent<Rigidbody>().useGravity = false;
        isThrowed = false;
        Material newMat = Resources.Load("Red", typeof(Material)) as Material; // DEFAULT
        System.Random rnd = new System.Random();
        int month = rnd.Next(1, 8);

        if (month == 1)
        {
            newMat = Resources.Load("Red", typeof(Material)) as Material;

        }
        else if (month == 2)
        {
            newMat = Resources.Load("Blue", typeof(Material)) as Material;
        }
        else if (month == 3)
        {
            newMat = Resources.Load("Pink", typeof(Material)) as Material;
        }
        else if (month == 4)
        {
            newMat = Resources.Load("Green", typeof(Material)) as Material;
        }
        else if (month == 5)
        {
            newMat = Resources.Load("NavyBlue", typeof(Material)) as Material;
        }
        else if (month == 6)
        {
            newMat = Resources.Load("Yellow", typeof(Material)) as Material;
        }
        else
        {
            newMat = Resources.Load("Orange", typeof(Material)) as Material;
        }
        objects.GetComponent<Renderer>().material = newMat;
        isPressedCreate = false;
        score++;
        textScoreChanger();
        if(turn == 1)
        {
            turn = 2;
        }
        else
        {
            turn = 1;
        }

    }

    void soundCreate()
    {
        createObjectSound.Play();
    }

    void buttonSoundCreate()
    {
        buttonSound.Play();
    }

    void successSoundCreate()
    {
        winPanel.SetActive(true);
        successSound.Play();
        Invoke("loadGameScene", 3);
    }

    public void gameOverSoundCreate()
    {
        objectIndex++;
        if (objectIndex == 3)
        {
            objectIndex = 0;
        }
        score = 0;
        //Debug.Log("Carpiistti");
        isGameOver = true;
        //loadGameScene();
    }

    public void restart()
    {
        objectIndex++;
        if (objectIndex == 3)
        {
            objectIndex = 0;
        }
        score = 0;
        loadGameScene();
    }

    public void loadGameScene()
    {
        SceneManager.LoadScene(2);
    }

    public void loadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    public void loadingGameScene()
    {
        buttonSoundCreate();
        WaitForSecondsDo(4f);
 
        SceneManager.LoadScene(2);

    }

    public void loadInfoScene()
    {
        buttonSoundCreate();
        SceneManager.LoadScene(2);
    }

    public void gameOver()
    {
        GameOverSoundPlay();
        //  Invoke("restart", 2);

    }

    public void GameOverSoundPlay()
    {
        //Debug.Log("Müzik çaldı");
        gameFinished.Play();
    }

    void textScoreChanger()
    {
        txtScore.text = score.ToString();
    }

    IEnumerator WaitForSecondsDo(float secToWait)
    {
        yield return new WaitForSeconds(secToWait);
    }

    private static void DataHandling()
    {
        stage = PlayerPrefs.GetInt("KeyOne");
        level = PlayerPrefs.GetInt("KeyTwo");
        if (stage == 0)
            stage = 1;
        if (level == 0)
            level = 1;
    }

    public void statReset()
    {
        PlayerPrefs.SetInt("KeyOne", 0);
        PlayerPrefs.SetInt("KeyTwo", 0);
    }


    float duration = 1.5f;
    private float t = 1.5f;
    bool isReset = false;
    bool colorTored = false;



    void ColorChangerr()
    {
        //   if (this.tag == "Barrier")
        // {
        //Material materialRainbow;
        // materialRainbow = Resources.Load("Rainbow.mat", typeof(Material)) as Material;
        if (stage == 1)
        {
            kasem1.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem2.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem3.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
        }
        else if (stage == 2)
        {
            kasem4.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem5.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem6.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);

        }
        else if (stage == 3)
        {
            kasem7.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem8.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem9.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
        }
        else if (stage == 4)
        {
            kasem10.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem11.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem12.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
        }
        else
        {
            kasem13.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem14.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
            kasem15.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, t);
        }

        // materialRainbow.color = Color.Lerp(Color.yellow, Color.red, t);

        //objects.GetComponent<Renderer>().material = newMat;
        if (!colorTored && turn == 1)
        {
            t += Time.deltaTime / duration;
            colorTored = false;
            if (t > 1.5f)
            {
                colorTored = true;
            }
        }
        else if (colorTored && turn == 2)
        {
            t -= Time.deltaTime / duration;
            colorTored = true;
            if (t < 0f)
            {
                colorTored = false;
            }
        }
        //   }

    }
    private void initializeAds()
    {
        Advertisement.AddListener((IUnityAdsListener)this);
        Advertisement.Initialize(gId, true);
    }
}
