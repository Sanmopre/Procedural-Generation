using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    private PerlinNoiseGenerator generator;
    int[] triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        generator = GetComponent<PerlinNoiseGenerator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        generator.CalcNoise();

        CreateMeshData();
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
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
