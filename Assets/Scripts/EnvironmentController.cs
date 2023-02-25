using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Skybox")]
    public Material skybox;
    public float sunSize;
    public float sunSizeConvergence;
    public float atmosphereThickness;
    public Color skyTint;
    public Color ground;
    public float exposure;

    [Header("Fog")]
    public Color dryFog;
    public Color wetFog;
    public bool isDry;
    public bool isWet;
    public float dryFogDensity = 0.005f;
    public float wetFogDensity = 0.0025f;
    public float fogDensity;

    [Header("Terrain")]
    public TerrainLayer trLayer1;
    public TerrainLayer trLayer2;
    public Texture2D dryTexture1;
    public Texture2D dryTexture2;
    public Texture2D wetTexture1;
    public Texture2D wetTexture2;

    private void Start()
    {
        skybox = RenderSettings.skybox;
        sunSize = skybox.GetFloat("_SunSize");
        sunSizeConvergence = skybox.GetFloat("_SunSizeConvergence");
        atmosphereThickness = skybox.GetFloat("_AtmosphereThickness");
        skyTint = skybox.GetColor("_SkyTint");
        ground = skybox.GetColor("_GroundColor");
        exposure = skybox.GetFloat("_Exposure");

        fogDensity = RenderSettings.fogDensity;
        if (isDry)
        {
            if (RenderSettings.fogColor != dryFog)
            {
                RenderSettings.fogColor = dryFog;
            }
            RenderSettings.fogDensity = dryFogDensity;
            trLayer1.diffuseTexture = dryTexture1;
            trLayer2.diffuseTexture = dryTexture2;

        }
        if (isWet)
        {
            if (RenderSettings.fogColor != wetFog)
            {
                RenderSettings.fogColor = wetFog;
            }
            RenderSettings.fogDensity = wetFogDensity;
            trLayer1.diffuseTexture = wetTexture1;
            trLayer2.diffuseTexture = wetTexture2;
        }
    }

    private void Update()
    {
        if (isDry)
        {
            if (RenderSettings.fogColor != dryFog)
            {
                RenderSettings.fogColor = dryFog;
            }
            RenderSettings.fogDensity = dryFogDensity;
            trLayer1.diffuseTexture = dryTexture1;
            trLayer2.diffuseTexture = dryTexture2;

        }
        if (isWet)
        {
            if (RenderSettings.fogColor != wetFog)
            {
                RenderSettings.fogColor = wetFog;
            }
            RenderSettings.fogDensity = wetFogDensity;
            trLayer1.diffuseTexture = wetTexture1;
            trLayer2.diffuseTexture = wetTexture2;
        }
    }
}
