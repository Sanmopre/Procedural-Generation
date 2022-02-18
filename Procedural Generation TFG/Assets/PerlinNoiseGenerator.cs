using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class PerlinNoiseGenerator : MonoBehaviour
{
    public GameObject test;
    public int gridSize;
    public float scale;
    public int sizeOfGrid;
    public float amplitudMultiplier;

    public Vector3[] vertices;
    public int[] triangles;

    void Start()
    {
        //CalcNoise();
    }

    public void CalcNoise()
    {
        triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
        vertices = new Vector3[gridSize * gridSize];

        for (int i = 0; i < gridSize; i++) 
        {
            for (int k = 0; k < gridSize; k++)
            {

                //SETTING VERTEX
                float sampleX = k / scale;
                float sampleY = i / scale;
                vertices[i + (k*gridSize)] = new Vector3(sizeOfGrid * i, Mathf.PerlinNoise(sampleX, sampleY) * amplitudMultiplier, sizeOfGrid * k);
            }
        }

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {

                //SETTING TRIANGLES
                int fixedGridSize = gridSize - 1;
                int index = (i * fixedGridSize + k) * 6;

                triangles[index] = (i * gridSize + k);
                triangles[index + 1] = ((i + 1) * gridSize + k);
                triangles[index + 2] = ((i + 1) * gridSize + k + 1);

                triangles[index + 3] = (i * gridSize + k);
                triangles[index + 4] = ((i + 1) * gridSize + k + 1);
                triangles[index + 5] = (i * gridSize + k + 1);
            }
        }
    }

    public Vector3[] GetVertices() 
    {
        return vertices;
    }

    public int[] GetTriangles()
    {
        return triangles;
    }
}