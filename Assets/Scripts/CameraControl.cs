using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Camera_Target;
    public float zoom;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        zoom = Mathf.Abs(Camera.main.transform.position.z - Camera_Target.transform.position.z);
    }
}
