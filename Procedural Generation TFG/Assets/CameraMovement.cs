using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public Vector3 pointToLook;
    void Update()
    {
        transform.RotateAround(pointToLook, Vector3.up, 20 * Time.deltaTime);
        transform.LookAt(pointToLook);
    }
}