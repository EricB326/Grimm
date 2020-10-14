using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Renderer m = new Renderer();
        Texture2D texture = new Texture2D(25, 25, TextureFormat.ARGB32, false);

        texture.GetPixels32();
    }


}
