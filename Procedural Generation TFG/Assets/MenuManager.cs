using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public Slider scaleSlider;
    public TMP_InputField seed;
    public Slider octaves;
    public Slider persistance;
    public Slider Lacunarity;
    public Slider amplitude;
    public Slider numberOfPoints;

    public Slider mixProportion;
    public TMP_Text mixProportion_text;

    public PerlinNoiseGenerator generator;
    public MeshGenerator meshGen;
    public TMP_Dropdown dropdown;


    public TMP_Text scale_text;
    public TMP_Text octaves_text;
    public TMP_Text persistance_text;
    public TMP_Text Lacunarity_text;
    public TMP_Text amplitude_text;
    public TMP_Text numberOfPoints_text;

    void Update()
    {
        generator.scale = scaleSlider.value;
        scale_text.SetText(scaleSlider.value.ToString());

        generator.octaves = (int)(octaves.value);
        octaves_text.SetText(octaves.value.ToString());

        generator.persistance = persistance.value;
        persistance_text.SetText(persistance.value.ToString());

        meshGen.noiseMode = dropdown.value;


        generator.lacunarity = Lacunarity.value;
        Lacunarity_text.SetText(Lacunarity.value.ToString());

        generator.numberOfPoints = (int)numberOfPoints.value;
        numberOfPoints_text.SetText(numberOfPoints.value.ToString());

        meshGen.meshAmplitudMultiplier = amplitude.value;
        amplitude_text.SetText(amplitude.value.ToString());


        generator.mixProportion = mixProportion.value;
        mixProportion_text.SetText(mixProportion.value.ToString());
    }
}
