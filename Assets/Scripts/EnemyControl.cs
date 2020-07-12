using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private GameObject target;
    private float shoot_delay, timeBetweenShots = 50.0f;
    private float wounds = 0, health = 100;
    private bool inCapturePoint = false;
    public float speed = 50;
    public AudioSource Engine, Damage, Shot, Shield;
    public GameObject Explosion;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.PAUSED)
        {
            //Rotates Saucer
            float rotx = transform.GetChild(0).localEulerAngles.x; float roty = transform.GetChild(0).localEulerAngles.y; float rotz = transform.GetChild(0).localEulerAngles.z;
            roty += 1.0f;
            if (roty > 360) roty = 1.0f;
            transform.GetChild(0).localEulerAngles = new Vector3(rotx, roty, rotz);

            //Saucer fires at Player
            if (shoot_delay > 0) shoot_delay -= 0.5f;
            if (shoot_delay <= 0)
            {
                if (Vector3.Distance(transform.position, GameManager.PLAYER.transform.position) < 100)
                {
                    transform.LookAt(GameManager.PLAYER.transform);
                    for (int i = 0; i < GameManager.FOE_BULLET.Length; i++)
                    {
                        if (!GameManager.FOE_BULLET[i].activeSelf)
                        {
                            GameManager.FOE_BULLET[i].SetActive(true);
                            GameManager.FOE_BULLET[i].transform.position = transform.position;
                            GameManager.FOE_BULLET[i].transform.rotation = transform.rotation;
                            if (Shot.isActiveAndEnabled) Shot.Play();
                            i = GameManager.FOE_BULLET.Length + 1;
                        }
                    }
                    shoot_delay = timeBetweenShots;
                }
            }

            //Small chance to switch targets, keep things interesting
            if (Random.Range(0, 100) == 0) PickTarget();

            //Check health vs wounds to kill object
            if (wounds > health)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                GameManager.POINTS += 1000;
                if(inCapturePoint) GameManager.GAME.loseCP_UI.gameObject.SetActive(false);
            }
        }
    }
    public GameObject FindPlayer()
    {
        GameObject go;
        //Debug.Log("Hunting Player");
        go = GameManager.PLAYER;
        return go;
    }
    public GameObject FindClosestCP()    
    {
        GameObject tgo;
        //Debug.Log("Hunting Nearest Beacon");
        GameObject[] go = GameObject.FindGameObjectsWithTag("Control_Point");
        tgo = go[0];
        for(int i = 1; i < go.Length; i++)
        {
            if (Vector3.Distance(transform.position, go[i].transform.position) < Vector3.Distance(transform.position, tgo.transform.position)) tgo = go[i];
        }
        return tgo;
    }
    public GameObject FindEnemyCP()
    {
        GameObject tgo;
        //Debug.Log("Hunting Player's Beacons");
        GameObject[] go = GameObject.FindGameObjectsWithTag("Control_Point");
        tgo = go[0];
        for (int i = 1; i < go.Length; i++)
        {
            if (Vector3.Distance(transform.position, go[i].transform.position) < Vector3.Distance(transform.position, tgo.transform.position) && go[i].GetComponent<CP_Controller>().owner == "Player") tgo = go[i];
        }
        if (tgo.GetComponent<CP_Controller>().owner != "Player") tgo = GameManager.PLAYER;
        return tgo;
    }
    public GameObject FindFriendlyCP()
    {
        GameObject tgo;
        //Debug.Log("Defending our Beacons");
        GameObject[] go = GameObject.FindGameObjectsWithTag("Control_Point");
        tgo = go[0];
        for (int i = 1; i < go.Length; i++)
        {
            if (Vector3.Distance(transform.position, go[i].transform.position) < Vector3.Distance(transform.position, tgo.transform.position) && go[i].GetComponent<CP_Controller>().owner == "Aliens") tgo = go[i];
        }
        if (tgo.GetComponent<CP_Controller>().owner != "Aliens") tgo = GameManager.PLAYER;
        return tgo;
    }

    public void PickTarget()
    {
        GameObject go = null;
        int choice = Random.Range(1, 8);
        if (choice < 6) go = GetComponent<EnemyControl>().FindPlayer();
        if (choice == 6) go = GetComponent<EnemyControl>().FindClosestCP();
        if (choice == 7) go = GetComponent<EnemyControl>().FindEnemyCP();
        if (choice == 8) go = GetComponent<EnemyControl>().FindFriendlyCP();
        target = go;
    }

    void FixedUpdate()
    {
        if (!GameManager.PAUSED)
        {
            //establish distance-behaviors
            float safeDistance = 25;
            Vector3 distance = target.transform.position - transform.position;
            if (target.tag != "Player") safeDistance = Random.Range(7f, 10f);

            //Face toward target
            transform.LookAt(target.transform);

            //Move either toward or around target
            if (Vector3.Distance(transform.position, target.transform.position) > safeDistance) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
            if (Vector3.Distance(transform.position, target.transform.position) <= safeDistance) { transform.Translate((Vector3.right * speed * Time.deltaTime) / 4); }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player_Shot" || col.transform.tag == "Blue_Shot")
        {
            col.transform.GetComponent<ShotController>().ResetShot();
            if (col.transform.tag == "Player_Shot" && transform.tag == "Blue_Enemy" && Shield.isActiveAndEnabled) Shield.Play(); //Red Bullet vs Blue Shield
            if (col.transform.tag == "Blue_Shot" && transform.tag == "Enemy" && Shield.isActiveAndEnabled) Shield.Play(); //Blue Bullet vs Red Shield
            if((col.transform.tag == "Player_Shot" && transform.tag == "Enemy") || (col.transform.tag == "Blue_Shot" && transform.tag == "Blue_Enemy")) //Red on Red or Blue on Blue
            {
                if (Damage.isActiveAndEnabled) Damage.Play();
                wounds += 40;
            }
        }
        if (col.transform.tag == "Control_Point") inCapturePoint = true;
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Control_Point") inCapturePoint = false;
    }
}
