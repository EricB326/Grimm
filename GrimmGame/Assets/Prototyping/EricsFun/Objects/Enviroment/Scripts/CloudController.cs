using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] private int horizontalStackSize = 20;
    [SerializeField] private float cloudHeight = 1f;
    [SerializeField] private Mesh quadMesh = null;
    [SerializeField] private Material cloudMaterial = null;
    private float offset = 0;

    [SerializeField] private int layer = 0;
    [SerializeField] private Camera camera = null;
    private Matrix4x4 matrix = Matrix4x4.zero;

    // Update is called once per frame
    void Update()    
    {
        cloudMaterial.SetFloat("_MidYValue", transform.position.y);
        cloudMaterial.SetFloat("_CloudHeight", cloudHeight);

        offset = cloudHeight / horizontalStackSize / 2f;
        Vector3 startPosition = transform.position + (Vector3.up * (offset * horizontalStackSize / 2f));
		for (int i = 0; i < horizontalStackSize; i++)
		{
            matrix = Matrix4x4.TRS(startPosition - (Vector3.up * offset * i), transform.rotation, transform.localScale);
            Graphics.DrawMesh(quadMesh, matrix, cloudMaterial, layer, camera, 0, null, true, false, false);
		}
    }
}
