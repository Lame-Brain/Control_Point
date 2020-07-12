using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScreenController : MonoBehaviour
{
    public Transform grid1, grid2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, 0.05f);
        grid1.RotateAround(grid1.position, Vector3.up, 0.005f);
        grid2.RotateAround(grid2.position, Vector3.up, -0.005f);
        if(Input.anyKey || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
