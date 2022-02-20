using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    private PerlinNoiseGenerator generator;
    private NoiseTextureGenerator textGen;
    public float meshAmplitudMultiplier;

    Mesh mesh;
    int[] triangles;
    Vector3[] vertices;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        generator = GetComponent<PerlinNoiseGenerator>();
        textGen = GetComponent<NoiseTextureGenerator>();     
    }

    // Start is called before the first frame update
    void Start()
    {
        generator.CalcNoise();
        CreateMeshData();
        textGen.DrawNoiseMap(vertices, (int)Mathf.Sqrt(vertices.Length));
        CreateMesh();

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
}
