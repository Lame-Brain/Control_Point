using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipControl : MonoBehaviour
{
    private float yaw, pitch, roll, speed;
    private Rigidbody rb;
    private float health = 100, wounds = 0, shield = 100, drain = 0, timeSinceDamage = 0, invinciblePeriod = 10;
    private bool inCapturePoint = false;
    private string capturePointOwner = "n.a";
    public float rotateSpeed = 45f;
    public float maxSpeed;
    public Transform redGun, bluGun, body;
    public float shoot_delay = 0;
    public GameObject Explosion;
    public float timeBetweenShots;

    //private float mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<AudioSource>().pitch = 0.5f;
        GetComponent<AudioSource>().Play();
        //mouseX = Input.mousePosition.x; mouseY = Input.mousePosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.PAUSED) GameManager.PLAYER.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (!GameManager.PAUSED)
        {
            GameManager.PLAYER.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            yaw = 0; pitch = 0; roll = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) yaw = -rotateSpeed;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) yaw = rotateSpeed;
            if (yaw == 0) body.localEulerAngles = new Vector3(body.localEulerAngles.x, body.localEulerAngles.y, 0);
            if (yaw > 0) body.localEulerAngles = new Vector3(body.localEulerAngles.x, body.localEulerAngles.y, -25);
            if (yaw < 0) body.localEulerAngles = new Vector3(body.localEulerAngles.x, body.localEulerAngles.y, 25);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) maxSpeed++;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) maxSpeed--;
            if (Input.GetKey(KeyCode.Alpha1)) { maxSpeed = 10; }
            if (Input.GetKey(KeyCode.Alpha2)) { maxSpeed = 20; }
            if (Input.GetKey(KeyCode.Alpha3)) { maxSpeed = 30; }
            if (Input.GetKey(KeyCode.Alpha4)) { maxSpeed = 40; }
            if (Input.GetKey(KeyCode.Alpha5)) { maxSpeed = 50; }
            if (Input.GetKey(KeyCode.Alpha6)) { maxSpeed = 60; }
            if (Input.GetKey(KeyCode.Alpha7)) { maxSpeed = 70; }
            if (Input.GetKey(KeyCode.Alpha8)) { maxSpeed = 80; }
            if (Input.GetKey(KeyCode.Alpha9)) { maxSpeed = 90; }
            if (Input.GetKey(KeyCode.Alpha0)) { maxSpeed = 100; }
            if (Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus)) maxSpeed += 10;
            if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus)) maxSpeed -= 10;
            if (maxSpeed > 100) maxSpeed = 100;
            if (maxSpeed < 10) maxSpeed = 10;
            

            if (shoot_delay > 0) shoot_delay -= 0.5f;
            if (shoot_delay <= 0)
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightAlt))
                {

                    for (int i = 0; i < GameManager.BLU_BULLET.Length; i++)
                    {
                        if (!GameManager.BLU_BULLET[i].activeSelf)
                        {
                            GameManager.BLU_BULLET[i].SetActive(true);
                            GameManager.BLU_BULLET[i].transform.position = bluGun.position;
                            GameManager.BLU_BULLET[i].transform.rotation = bluGun.rotation;
                            GameManager.BLU_BULLET[i].GetComponent<ShotController>().AddSpeed(rb.velocity.magnitude);
                            GameManager.GAME.firelaser.Play();
                            i = GameManager.BLU_BULLET.Length + 1;
                        }
                    }
                    shoot_delay = timeBetweenShots;
                }
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightControl))
                {
                    for (int i = 0; i < GameManager.RED_BULLET.Length; i++)
                    {
                        if (!GameManager.RED_BULLET[i].activeSelf)
                        {
                            GameManager.RED_BULLET[i].SetActive(true);
                            GameManager.RED_BULLET[i].transform.position = redGun.position;
                            GameManager.RED_BULLET[i].transform.rotation = redGun.rotation;
                            GameManager.RED_BULLET[i].GetComponent<ShotController>().AddSpeed(rb.velocity.magnitude);
                            GameManager.GAME.firelaser.Play();
                            i = GameManager.RED_BULLET.Length + 1;
                        }
                    }
                    shoot_delay = timeBetweenShots;
                }
            }

            //Engine Sounds
            if (maxSpeed < 11) GetComponent<AudioSource>().pitch = 0.5f;
            if (maxSpeed > 10 && maxSpeed < 21) GetComponent<AudioSource>().pitch = 0.6f;
            if (maxSpeed > 20 && maxSpeed < 31) GetComponent<AudioSource>().pitch = 0.7f;
            if (maxSpeed > 30 && maxSpeed < 41) GetComponent<AudioSource>().pitch = 0.8f;
            if (maxSpeed > 40 && maxSpeed < 51) GetComponent<AudioSource>().pitch = 0.9f;
            if (maxSpeed > 50 && maxSpeed < 61) GetComponent<AudioSource>().pitch = 1.0f;
            if (maxSpeed > 60 && maxSpeed < 71) GetComponent<AudioSource>().pitch = 1.1f;
            if (maxSpeed > 70 && maxSpeed < 81) GetComponent<AudioSource>().pitch = 1.3f;
            if (maxSpeed > 80 && maxSpeed < 91) GetComponent<AudioSource>().pitch = 1.4f;
            if (maxSpeed > 90) GetComponent<AudioSource>().pitch = 1.5f;

            //DamageTimer        
            timeSinceDamage++;
            if (timeSinceDamage > 1500f) drain--;
            if (drain < 0) drain = 0; if (drain > shield) drain = shield; if (wounds > health) wounds = health;
            if (inCapturePoint && capturePointOwner == "Player") { wounds--; if (wounds < 0) wounds = 0; }

            //watch for death
            if(wounds >= health)
            {
                GameManager.GAME.PlayerTakesDamage(5f);
                Instantiate(Explosion, transform.position, transform.rotation);
                Invoke("GoToDeadScreen", 5.5f);
            }
        }
    }

    //Fixed Update
    void FixedUpdate()
    {
        if (!GameManager.PAUSED)
        {
            //Rotate Ship        
            transform.rotation *= Quaternion.Euler(pitch * Time.deltaTime, yaw * Time.deltaTime, roll * Time.deltaTime);

            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
            float f = 1f; float r = 0.5f;
            if (speed + f < maxSpeed) speed += f;
            if (speed - r > maxSpeed) speed -= r;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    public void GoToDeadScreen() { SceneManager.LoadScene("Dead"); }

    public float HullPercent()
    {
        return ((health - wounds) / health) * 100;
    }
    public float ShieldPercent()
    {
        return ((shield - drain) / shield) * 100;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Enemy" || col.transform.tag == "Blue_Enemy")
        {
            transform.rotation = Quaternion.identity;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 0);
            if(timeSinceDamage > invinciblePeriod)
            {
                GameManager.GAME.PlayerTakesDamage(1f);
                timeSinceDamage = 0;
                if(drain < shield) drain += 75;
                if (drain >= shield) wounds += 75;
            }
            
        }
    }
    public void OnTriggerEnter(Collider col)
    {        
        if (col.transform.tag == "Foe_Shot")
        {
            col.transform.GetComponent<ShotController>().ResetShot();
            if (timeSinceDamage > invinciblePeriod)
            {
                GameManager.GAME.PlayerTakesDamage(1f);
                timeSinceDamage = 0;
                Debug.Log("shields = " + (shield - drain).ToString() + " and hull = " + (health - wounds).ToString());
                if (drain < shield) drain += 33;
                if (drain >= shield) wounds += 25;
            }
        }
        if (col.transform.tag == "Control_Point")
        {
            inCapturePoint = true;
            capturePointOwner = col.transform.GetComponent<CP_Controller>().owner;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Control_Point")
        {
            inCapturePoint = false;
            capturePointOwner = "n.a";
        }
    }
}
