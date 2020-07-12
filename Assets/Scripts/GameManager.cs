using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameObject[] RED_BULLET, BLU_BULLET, FOE_BULLET;    
    public static GameObject PLAYER;
    public static GameManager GAME;
    public static int POINTS = 0, HISCORE = 0;
    public static bool PAUSED;

    public GameObject player_PF, redBullPF, bluBullPF, redUfoPF, bluUfoPF, foeBullPF, cp_PF;
    public GameObject[] ControlPoint;
    public Light mainLight;
    public GameObject stars, grid;
    public int max_bullets, numberControlPoints;
    private float spawnBubble = 100;
    private int waveNum, waveSize;

    public Text pointsUI, HiScoreUI, engineUI, speedUI, gainCP_UI, loseCP_UI, friend_UI, foe_UI, Message_UI, shield_UI, hull_UI;
    public GameObject visibleImpact, pauseMenu_UI;

    //Story Variables
    private bool played1Beacon, played3Beacon, playedScogBeacon, playedScog3Beacon;
    [HideInInspector]
    public int beaconsCaptured;
    [HideInInspector]
    public int beaconsLost;

    //public AudioSource beep, buzz, confirm, damage, firelaser, shipengine;
    public AudioSource beep, buzz, confirm, damage, firelaser, beacon1, beacon3, scogBeacon, scog3Beacon;


    void Awake()
    {
        GAME = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get stored HISCORE
        if (PlayerPrefs.HasKey("HISCORE")) HISCORE = PlayerPrefs.GetInt("HISCORE");

        PLAYER = Instantiate(player_PF, Vector3.zero, Quaternion.identity);
        PAUSED = false;
        waveNum = 0;
        waveSize = 1;
        RED_BULLET = new GameObject[max_bullets];
        BLU_BULLET = new GameObject[max_bullets];
        FOE_BULLET = new GameObject[max_bullets * 10];
        for (int i = 0; i < max_bullets; i++)
        {
            RED_BULLET[i] = Instantiate(redBullPF); RED_BULLET[i].SetActive(false);
            BLU_BULLET[i] = Instantiate(bluBullPF); BLU_BULLET[i].SetActive(false);
        }
        for (int i = 0; i < max_bullets * 10; i++)
        {
            FOE_BULLET[i] = Instantiate(foeBullPF); FOE_BULLET[i].SetActive(false);
        }        
        ControlPoint = new GameObject[numberControlPoints];
        for(int i = 0; i < numberControlPoints; i++)
        {
            ControlPoint[i] = Instantiate(cp_PF, new Vector3(Random.Range(-1000f, 1000f), -1, Random.Range(-1000f, 1000f)), cp_PF.transform.rotation);            
        }
        played1Beacon = false;
        played3Beacon = false;
        playedScogBeacon = false;
        playedScog3Beacon = false;
        

        InvokeRepeating("SpawnEnemies", 20, 30);
    }

    // Update is called once per frame
    void Update()
    {
        //Updates UI
        HiScoreUI.text = "HI: " + HISCORE.ToString();
        pointsUI.text = "Pts: " + POINTS.ToString();
        engineUI.text = "Engine Setting: " + PLAYER.GetComponent<ShipControl>().maxSpeed.ToString();
        speedUI.text = "Current Speed: " + PLAYER.GetComponent<Rigidbody>().velocity.magnitude.ToString("F2");
        shield_UI.text = "Shield: " + PLAYER.GetComponent<ShipControl>().ShieldPercent().ToString() + "%";
        hull_UI.text = "Hull: " + PLAYER.GetComponent<ShipControl>().HullPercent().ToString() + "%";

        //keeps grid in view
        if (PLAYER.transform.position.x > stars.transform.position.x + 200)
        {
            stars.transform.position = new Vector3(stars.transform.position.x + 200, stars.transform.position.y, stars.transform.position.z);
            grid.transform.position = new Vector3(grid.transform.position.x + 200, grid.transform.position.y, grid.transform.position.z);
        }
        if (PLAYER.transform.position.x < stars.transform.position.x - 200)
        {
            stars.transform.position = new Vector3(stars.transform.position.x - 200, stars.transform.position.y, stars.transform.position.z);
            grid.transform.position = new Vector3(grid.transform.position.x - 200, grid.transform.position.y, grid.transform.position.z);
        }
        if (PLAYER.transform.position.z > stars.transform.position.z + 200)
        {
            stars.transform.position = new Vector3(stars.transform.position.x, stars.transform.position.y, stars.transform.position.z + 200);
            grid.transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y, grid.transform.position.z + 200);
        }
        if (PLAYER.transform.position.z < stars.transform.position.z - 200)
        {
            stars.transform.position = new Vector3(stars.transform.position.x, stars.transform.position.y, stars.transform.position.z - 200);
            grid.transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y, grid.transform.position.z - 200);
        }

        if (Input.GetKeyUp(KeyCode.Escape) && !PAUSED)
        {
            PauseGameToggle();
        }

        //Story Time!
        beaconsCaptured = 0; beaconsLost = 0;
        for(int i = 0; i < ControlPoint.Length; i++)
        {
            if (ControlPoint[i].GetComponent<CP_Controller>().owner == "Player") beaconsCaptured++;
            if (ControlPoint[i].GetComponent<CP_Controller>().owner == "Aliens") beaconsLost++;            
        }

        POINTS += beaconsCaptured * 10;

        if (beaconsCaptured == 1 && !played1Beacon)
        {
            played1Beacon = true;
            beacon1.Play();
        }
        if (beaconsCaptured == 3 && !played3Beacon)
        {
            played3Beacon = true;
            beacon3.Play();
        }
        if (beaconsLost == 1 && !playedScogBeacon)
        {
            playedScogBeacon = true;
            scogBeacon.Play();
        }
        if (beaconsLost == 3 && !playedScog3Beacon)
        {
            playedScog3Beacon = true;
            scog3Beacon.Play();
        }
        if (beaconsCaptured == 5)
        {
            POINTS += 1000000;
            WinGame();            
        }

        //TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    public void SpawnEnemies()
    {
        if (!PAUSED)
        {
            float x = Random.Range(0, 100); float y = Random.Range(0, 100);
            if (x < 50f) x -= spawnBubble; if (x >= 50f) x += spawnBubble;
            if (y < 50f) y -= spawnBubble; if (x >= 50f) y += spawnBubble;
            for (int i = 0; i < waveSize; i++)
            {
                int offsetX = Random.Range(1, 5); int offsetY = Random.Range(1, 5);
                int RedOrBlue = Random.Range(0, 100);
                GameObject go = new GameObject();
                if (RedOrBlue < 50) go = Instantiate(redUfoPF, new Vector3(x + offsetX, 0, y + offsetY), Quaternion.identity);
                if (RedOrBlue >= 50) go = Instantiate(bluUfoPF, new Vector3(x + offsetX, 0, y + offsetY), Quaternion.identity);
                go.GetComponent<EnemyControl>().PickTarget();
            }
            waveNum++;
            if (waveNum == 6) waveSize++;
            if (waveSize == 11) waveSize = 1;
        }
    }

    public void PlayerTakesDamage(float d)
    {
        if (!visibleImpact.activeInHierarchy)
        {
            visibleImpact.SetActive(true);
            Invoke("PlayerStopsTakingDamage", d);
            damage.Play();
        }
    }
    public void PlayerStopsTakingDamage()
    {
        visibleImpact.SetActive(false);
        damage.Stop();
    }

    public void PauseGameToggle()
    {
        pauseMenu_UI.SetActive(!pauseMenu_UI.activeInHierarchy);
        PAUSED = !PAUSED;
    }

    public void RestartGame()
    {
        if (POINTS > HISCORE) PlayerPrefs.SetInt("HISCORE", POINTS);
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        if (POINTS > HISCORE) PlayerPrefs.SetInt("HISCORE", POINTS);
        Application.Quit();
    }
    public void WinGame()
    {
        if (POINTS > HISCORE) PlayerPrefs.SetInt("HISCORE", POINTS);
        SceneManager.LoadScene("Win");
    }
}
