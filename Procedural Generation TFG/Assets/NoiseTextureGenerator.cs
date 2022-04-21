using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawNoiseMap(Vector3[] vertices, int gridSize)
    {
        Texture2D texture = new Texture2D(gridSize, gridSize);

        Color[] colorMap = new Color[gridSize*gridSize];

        for (int i = 0; i < gridSize * gridSize; i++) 
        {
            colorMap[i] = Color.Lerp(Color.black,Color.white,vertices[i].y);
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(35, 20, 35);
    }
}
