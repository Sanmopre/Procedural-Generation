using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseRepresentationInCamera : MonoBehaviour
{
    public GameObject plane;
    public Vector3 offset;
    public Vector3 rotationOffset;
    // Update is called once per frame

    void Update()
    {
        plane.transform.position = transform.position + offset;
        plane.transform.rotation =  Quaternion.Euler(rotationOffset + transform.rotation.eulerAngles);
        //plane.transform.LookAt(transform.position);
    }
}
