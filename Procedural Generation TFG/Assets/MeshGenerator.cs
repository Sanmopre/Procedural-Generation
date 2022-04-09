using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapInformation;

public class MeshGenerator : MonoBehaviour
{
    public GameObject meshObject;
    private PerlinNoiseGenerator generator;
    private NoiseTextureGenerator textGen;
    public float meshAmplitudMultiplier;

    public int sizeOfMap;

    GameObject[] meshArray;

    private void Awake()
    {        
        //Noise generator
        generator = GetComponent<PerlinNoiseGenerator>();

        //Noise texture generator
        textGen = GetComponent<NoiseTextureGenerator>();
        
        //Different meshes calculations
        meshArray = new GameObject[sizeOfMap * sizeOfMap];
        for (int i = 0; i < sizeOfMap; i++)
        {
            for (int k = 0; k < sizeOfMap; k++)
            {
                meshArray[i*sizeOfMap + k] = Instantiate(meshObject, new Vector3(k * (generator.gridSize - 1), 0, -i * (generator.gridSize - 1)), Quaternion.identity);
            }
        }

    }

    private void Start()
    {
        //First Generation of the map
        UpdateMap();
    }

    public void UpdateMap() 
    {
        textGen.DrawNoiseMap(generator.VoronoiNoise().vertices, (int)Mathf.Sqrt(generator.VoronoiNoise().vertices.Length));
        CreateMaps();
    }

    void CreateMaps() 
    {
        //Loop through the different meshes
        for (int i = 0; i < sizeOfMap; i++)
        {
            for (int k = 0; k < sizeOfMap; k++)
            {
                //Generate the Noise Values
                generator.position = new Vector2(-i * (generator.gridSize - 1), -k * (generator.gridSize - 1));
                MapInfo mapnInfo = generator.VoronoiNoise();

                Mesh pogMesh = meshArray[i * sizeOfMap + k].GetComponent<MeshFilter>().mesh;
                pogMesh.Clear();
                for (int s = 0; s < mapnInfo.vertices.Length; s++)
                {
                    mapnInfo.vertices[s].y *= meshAmplitudMultiplier;
                }
                pogMesh.vertices = mapnInfo.vertices;
                pogMesh.triangles = mapnInfo.triangles;
                pogMesh.RecalculateNormals();
            }
        }
    }
}
