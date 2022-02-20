using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class PerlinNoiseGenerator : MonoBehaviour
{
    
    public int gridSize;
 
    public int vertexDistance;

    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;

    private Vector3[] vertices;
    private int[] triangles;

    public void CalcNoise()
    {

        //PSEUDO RANDOM NUMBER GENERATOR BASED ON SEED
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) 
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }


        triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
        vertices = new Vector3[gridSize * gridSize];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;


        for (int i = 0; i < gridSize; i++) 
        {
            for (int k = 0; k < gridSize; k++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //SETTING VERTEX POSITIONS
                for (int p = 0; p < octaves; p++)
                {
                    float sampleX = k / scale * frequency + octaveOffsets[p].x;
                    float sampleY = i / scale * frequency + octaveOffsets[p].y;

                    //*2 - 1 so we can get also negative values
                    float perlinValue = Mathf.PerlinNoise(sampleX,sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) 
                {
                    minNoiseHeight = noiseHeight;
                }

                vertices[i + (k * gridSize)] = new Vector3(vertexDistance * i, noiseHeight, vertexDistance * k);
            }
        }

        //NORMALIZE VECTOR OF VERTICES
        for (int r = 0; r < vertices.Length; r++) 
        {
            float newY = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, vertices[r].y);
            vertices[r] = new Vector3(vertices[r].x,newY,vertices[r].z);
        }

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {
                //SETTING TRIANGLES INDEX
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