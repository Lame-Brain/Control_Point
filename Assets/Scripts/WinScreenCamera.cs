using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenCamera : MonoBehaviour
{
    public GameObject CP;
    public GameObject Credits, Other;
    public AudioSource WinMessage;

    // Start is called before the first frame update
    void Start()
    {
        WinMessage.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.LookAt(CP.transform);
        Camera.main.transform.Translate(Vector3.right * 0.03f);

        if (Input.anyKey || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            Credits.SetActive(true);
            Other.SetActive(false);
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
