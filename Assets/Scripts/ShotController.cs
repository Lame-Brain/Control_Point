using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float speed;
    private float actualSpeed = 0;
    public float life;
    private float frames;

    public void AddSpeed(float vel) { actualSpeed += vel; }
    public void ResetSpeed() { actualSpeed = speed; }
    public void ResetShot()
    {
        frames = 0;
        gameObject.SetActive(false);
        actualSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.PAUSED)
        {
            if (actualSpeed < speed) ResetSpeed();
            transform.Translate(Vector3.forward * actualSpeed * Time.deltaTime);
            frames++;
            if (frames > life) ResetShot();
        }
    }
}
