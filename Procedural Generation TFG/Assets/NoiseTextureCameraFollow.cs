using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTextureCameraFollow : MonoBehaviour
{
    private Camera camera;
    public GameObject noiseTexture;

    private void Awake()
    {
        camera = Camera.main;
    }
    void Update()
    {
        noiseTexture.transform.position = transform.position + transform.forward;
        noiseTexture.transform.LookAt(transform);
        //noiseTexture.transform.eulerAngles = new Vector3(noiseTexture.transform.eulerAngles.x, noiseTexture.transform.eulerAngles.y + 90, noiseTexture.transform.eulerAngles.z);

        Debug.Log(transform.forward);
    }
}
