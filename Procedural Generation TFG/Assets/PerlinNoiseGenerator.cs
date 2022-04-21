using UnityEngine;
using System.Collections;
using MapInformation;
// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

    enum NoiseType 
    {
    PERLIN,
    VORONOI,
    VORONOI_SECOND_POINT,
    VORONOI_SECOND_MINUS_FIRST_POINT,
    VORONOI_ONE_MINUS_FIRST,
    VORONOI_FIRST_DOT_SECOND
};

public class PerlinNoiseGenerator : MonoBehaviour
{
    public int gridSize;

    public int vertexDistance;

    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;


    float maxNoiseHeight;
    float minNoiseHeight;

    public Vector2 position;

    //Voronoi Noise
    public int numberOfPoints;
     
    public MapInfo CalcImprovedPerlinNoise()
    {
        MapInfo returnValue;

        //PSEUDO RANDOM NUMBER GENERATOR BASED ON SEED
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + position.x;
            float offsetY = prng.Next(-100000, 100000) - position.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }


        returnValue.triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
        returnValue.vertices = new Vector3[gridSize * gridSize];

        maxNoiseHeight = float.MinValue;
        minNoiseHeight = float.MaxValue;


        for (int i = 0; i < gridSize; i++)
        {
            for (int k = 0; k < gridSize; k++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                //SETTING VERTEX POSITIONS
                for (int p = 0; p < octaves; p++)
                {
                    float sampleX = (k + octaveOffsets[p].x) / scale * frequency;
                    float sampleY = (i + octaveOffsets[p].y) / scale * frequency;

                    //*2 - 1 so we can get also negative values
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

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

                returnValue.vertices[i + (k * gridSize)] = new Vector3(vertexDistance * i, noiseHeight, vertexDistance * k);
            }
        }


        //NORMALIZE VECTOR OF VERTICES
        for (int r = 0; r < returnValue.vertices.Length; r++)
        {
            float newY = (returnValue.vertices[r].y + 1) / (2f * maxPossibleHeight);
            returnValue.vertices[r] = new Vector3(returnValue.vertices[r].x, newY, returnValue.vertices[r].z);
        }

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {
                //SETTING TRIANGLES INDEX
                int fixedGridSize = gridSize - 1;
                int index = (i * fixedGridSize + k) * 6;

                returnValue.triangles[index] = (i * gridSize + k);
                returnValue.triangles[index + 1] = ((i + 1) * gridSize + k);
                returnValue.triangles[index + 2] = ((i + 1) * gridSize + k + 1);

                returnValue.triangles[index + 3] = (i * gridSize + k);
                returnValue.triangles[index + 4] = ((i + 1) * gridSize + k + 1);
                returnValue.triangles[index + 5] = (i * gridSize + k + 1);
            }
        }
        return returnValue;
    }


    public MapInfo LeveledPerlinNoise() 
    {
        MapInfo mapInfo;
        mapInfo.vertices = new Vector3[gridSize * gridSize];
        mapInfo.triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];

        for (int i = 0; i < gridSize; i++)
        {
            for (int k = 0; k < gridSize; k++)
            {
                float perlinValue1 = Mathf.PerlinNoise((float)(i + position.x) / scale, (float)(k - position.y) / scale);

                float perlinValue = Mathf.Floor(perlinValue1 * 10.0f);//Mathf.Clamp(perlinValue1,0.3f,0.7f);//Mathf.Floor(perlinValue1 * 10.0f);

                if (perlinValue > maxNoiseHeight)
                {
                    maxNoiseHeight = perlinValue;
                }
                else if (perlinValue < minNoiseHeight)
                {
                    minNoiseHeight = perlinValue;
                }

                mapInfo.vertices[i + (k * gridSize)] = new Vector3(i, perlinValue, k);


            }
        }

        //NORMALIZE VECTOR OF VERTICES
        for (int r = 0; r < mapInfo.vertices.Length; r++)
        {
            float newValue = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, mapInfo.vertices[r].y);
            mapInfo.vertices[r] = new Vector3(mapInfo.vertices[r].x, newValue, mapInfo.vertices[r].z);
        }

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {
                //SETTING TRIANGLES INDEX
                int fixedGridSize = gridSize - 1;
                int index = (i * fixedGridSize + k) * 6;

                mapInfo.triangles[index] = (i * gridSize + k);
                mapInfo.triangles[index + 1] = ((i + 1) * gridSize + k);
                mapInfo.triangles[index + 2] = ((i + 1) * gridSize + k + 1);

                mapInfo.triangles[index + 3] = (i * gridSize + k);
                mapInfo.triangles[index + 4] = ((i + 1) * gridSize + k + 1);
                mapInfo.triangles[index + 5] = (i * gridSize + k + 1);
            }
        }

        return mapInfo;
    }

    public MapInfo VoronoiNoise() 
    {
        MapInfo mapInfo;
        maxNoiseHeight = float.MinValue;
        minNoiseHeight = float.MaxValue;

        mapInfo.vertices = new Vector3[gridSize * gridSize];
        mapInfo.triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];

        Vector2[] points = new Vector2[numberOfPoints * numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++) 
        {
            for (int k = 0; k < numberOfPoints; k++)
             {
                points[k + i * numberOfPoints] = new Vector2(Random.Range(0, gridSize), Random.Range(0, gridSize));
            }
        }


        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {
                float distanceValue = float.MaxValue;
                float secondDistance = float.MaxValue;
                for (int s = 0; s < numberOfPoints * numberOfPoints; s++) 
                {
                    float newDistance = Vector2.Distance(points[s], new Vector2(i, k));

                    if (newDistance < distanceValue) 
                    {
                        distanceValue = newDistance;
                    }
                }


                for (int s = 0; s < numberOfPoints * numberOfPoints; s++)
                {
                    float newDistance = Vector2.Distance(points[s], new Vector2(i, k));

                    if (newDistance < secondDistance && newDistance > distanceValue)
                    {
                        secondDistance = newDistance;
                    }
                }

                float pogValue = Mathf.Abs( secondDistance * distanceValue);

                if (pogValue > maxNoiseHeight)
                {
                    maxNoiseHeight = pogValue;
                }
                else if (pogValue < minNoiseHeight)
                {
                    minNoiseHeight = pogValue;
                }

                mapInfo.vertices[i + (k * gridSize)] = new Vector3(i, pogValue, k);
            }
        }


        //NORMALIZE VECTOR OF VERTICES
        for (int r = 0; r < mapInfo.vertices.Length; r++)
        {
            float newValue = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, mapInfo.vertices[r].y);
            mapInfo.vertices[r] = new Vector3(mapInfo.vertices[r].x, newValue, mapInfo.vertices[r].z);
        }

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {
                //SETTING TRIANGLES INDEX
                int fixedGridSize = gridSize - 1;
                int index = (i * fixedGridSize + k) * 6;

                mapInfo.triangles[index] = (i * gridSize + k);
                mapInfo.triangles[index + 1] = ((i + 1) * gridSize + k);
                mapInfo.triangles[index + 2] = ((i + 1) * gridSize + k + 1);

                mapInfo.triangles[index + 3] = (i * gridSize + k);
                mapInfo.triangles[index + 4] = ((i + 1) * gridSize + k + 1);
                mapInfo.triangles[index + 5] = (i * gridSize + k + 1);
            }
        }
        return mapInfo;
    }


    public MapInfo MixVoronoiAndPerlin() 
    {
        MapInfo mapInfo;

        mapInfo.vertices = new Vector3[gridSize * gridSize];
        mapInfo.triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];

        MapInfo Perlin = CalcImprovedPerlinNoise();
        MapInfo Voronoi = VoronoiNoise();

        for (int r = 0; r < mapInfo.vertices.Length; r++)
        {
            float newY = (Perlin.vertices[r].y * Voronoi.vertices[r].y);
            mapInfo.vertices[r] = new Vector3(mapInfo.vertices[r].x,newY, mapInfo.vertices[r].z);
        }

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int k = 0; k < gridSize - 1; k++)
            {
                //SETTING TRIANGLES INDEX
                int fixedGridSize = gridSize - 1;
                int index = (i * fixedGridSize + k) * 6;

                mapInfo.triangles[index] = (i * gridSize + k);
                mapInfo.triangles[index + 1] = ((i + 1) * gridSize + k);
                mapInfo.triangles[index + 2] = ((i + 1) * gridSize + k + 1);

                mapInfo.triangles[index + 3] = (i * gridSize + k);
                mapInfo.triangles[index + 4] = ((i + 1) * gridSize + k + 1);
                mapInfo.triangles[index + 5] = (i * gridSize + k + 1);
            }
        }

        return mapInfo;
    }

}