using System.Collections;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] NoiseMapGenerator(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        if (scale <= 0)
        {
            Debug.LogWarning("NoiseMapGenerator::WARNING::Scale parameter was less than or equal to 0");
            scale = 0.0001f;
        }

        float[,] noiseMap = new float[width, height];

        System.Random psuedoRand = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = psuedoRand.Next(-100000, 100000) + offset.x;
            float offsetY = psuedoRand.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = width / 2.0f;
        float halfHeight = height / 2.0f;

        for (int y = 0; y < height; y++)
            for(int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    // In range of [-1, 1]
                    float perlinValue = Mathf.PerlinNoise(sX, sY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    // Amplitude will decrease as persistance is [0, 1]
                    amplitude *= persistance;

                    // Frequency will increase
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
                noiseMap[x, y] = noiseHeight;
            }

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                // InverseLerp returns a value [0, 1]
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }

        return noiseMap;
    }
}
