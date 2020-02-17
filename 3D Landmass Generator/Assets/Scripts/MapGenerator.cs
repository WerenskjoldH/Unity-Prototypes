using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    public string name;
    [Range(0, 1)]
    public float height;
    public Color color;
}

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NOISE,
        COLOR,
        MESH
    }

    const int mapChunkSize = 241;
    // LOD
    [Range(0,6)]
    public int levelOfDetail;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightModifer;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdateMap;

    public DrawMode drawMode;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = NoiseGenerator.NoiseMapGenerator(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for(int y = 0; y < mapChunkSize; y++)
            for(int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }

        DisplayMap display = FindObjectOfType<DisplayMap>();
        if (drawMode == DrawMode.COLOR)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.NOISE)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.MESH)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightModifer, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}
