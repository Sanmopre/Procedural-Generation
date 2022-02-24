using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public GameObject meshObject;
    private PerlinNoiseGenerator generator;
    private NoiseTextureGenerator textGen;
    public float meshAmplitudMultiplier;

    public int sizeOfMap;

    GameObject[] meshArray;

    Mesh mesh;
    int[] triangles;
    Vector3[] vertices;

    private void Awake()
    {        
        generator = GetComponent<PerlinNoiseGenerator>();
        textGen = GetComponent<NoiseTextureGenerator>();         
        meshArray = new GameObject[sizeOfMap * sizeOfMap];
        for (int i = 0; i < sizeOfMap; i++)
        {
            for (int k = 0; k < sizeOfMap; k++)
            {
                meshArray[i*sizeOfMap + k] = Instantiate(meshObject, new Vector3(k * (generator.gridSize - 1), 0, -i * (generator.gridSize - 1)), Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        generator.CalcNoise();
        CreateMeshData();
        textGen.DrawNoiseMap(vertices, (int)Mathf.Sqrt(vertices.Length));
        CreateMaps();
    }

    void CreateMeshData() 
    {
        vertices = generator.GetVertices();
        triangles = generator.GetTriangles();
    }

    void CreateMesh() 
    {
        mesh.Clear();
        for (int i = 0; i < vertices.Length; i++) 
        {
            vertices[i].y *= meshAmplitudMultiplier; 
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void CreateMaps() 
    {
        for (int i = 0; i < sizeOfMap; i++)
        {
            for (int k = 0; k < sizeOfMap; k++)
            {
                generator.position = new Vector2(-i * generator.gridSize, -k * generator.gridSize);
                generator.CalcNoise();
                CreateMeshData();

                vertices = generator.GetVertices();
                triangles = generator.GetTriangles();

                Mesh pogMesh = meshArray[i * sizeOfMap + k].GetComponent<MeshFilter>().mesh;
                pogMesh.Clear();
                for (int s = 0; s < vertices.Length; s++)
                {
                    vertices[s].y *= meshAmplitudMultiplier;
                }
                pogMesh.vertices = vertices;
                pogMesh.triangles = triangles;
                pogMesh.RecalculateNormals();
            }
        }
    }
}
