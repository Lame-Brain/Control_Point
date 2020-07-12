using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject Dolly;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (GameObject.FindGameObjectWithTag("Dolly").activeInHierarchy)
        {
            Dolly = GameObject.FindGameObjectWithTag("Dolly");
            transform.position = Dolly.transform.position;
            transform.rotation = Dolly.transform.rotation;
        }
    }
}
