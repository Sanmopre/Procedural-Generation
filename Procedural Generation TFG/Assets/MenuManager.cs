using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Slider scaleSlider;
    public Slider octaves;
    public Slider persistance;
    public Slider Lacunarity;
    public Slider amplitude;
    public PerlinNoiseGenerator generator;
    public MeshGenerator meshGen;

    void Update()
    {
        generator.scale = scaleSlider.value;
        generator.octaves = (int)(octaves.value);
        generator.persistance = persistance.value;
        generator.lacunarity = Lacunarity.value;
        meshGen.meshAmplitudMultiplier = amplitude.value;
    }
}
