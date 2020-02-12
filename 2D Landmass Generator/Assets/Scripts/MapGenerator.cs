﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdateMap;

    public void GenerateMap()
    {
        float[,] noiseMap = NoiseGenerator.NoiseMapGenerator(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        DisplayMap display = FindObjectOfType<DisplayMap>();
        display.DrawNoiseMap(noiseMap);
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
       
    }
}
