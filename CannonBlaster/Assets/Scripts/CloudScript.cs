using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    Light light;

    [SerializeField]
    int cloudTextureResolution = 256;
    Texture2D cloudTexture;

    [SerializeField]
    float cloudSpeed = 0.5f;

    // Look into using a command buffer, we should be able to get real-time dynamic cloud shadows working through that
    void InitializeCloudTexture()
    {
        Color tempColor;
        float pixelValue;

        for(int i = 0; i < cloudTextureResolution; i++)
            for(int j = 0; j < cloudTextureResolution; j++)
            {
                //pixelValue = Mathf.PerlinNoise(i * 10.0f, j * 10.0f);
                pixelValue = (i + Time.time) / (float)cloudTextureResolution * 255.0f;
                tempColor.r = pixelValue;
                tempColor.g = pixelValue;
                tempColor.b = pixelValue;
                tempColor.a = pixelValue;

                cloudTexture.SetPixel(i, j, tempColor);
            }

        cloudTexture.Apply();
    }

    void Start()
    {
        //light = GetComponent<Light>();

        //cloudTexture = new Texture2D(cloudTextureResolution, cloudTextureResolution, TextureFormat.Alpha8, false);
        //cloudTexture.name = "Cloud Texture";
        //cloudTexture.wrapMode = TextureWrapMode.Repeat;

        ////InitializeCloudTexture();

        //light.cookie = cloudTexture;
    }

    void Update()
    {
        //CalcNoise();
        //light.cookie = cloudTexture;
        transform.position = transform.position + new Vector3(cloudSpeed * Time.deltaTime, 0, 0);
    }
}
