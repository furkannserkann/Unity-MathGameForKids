using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMove : MonoBehaviour
{
    private MeshRenderer mesh;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Scrool();
    }

    void Scrool()
    {
        //speed += 0.0001f * Time.deltaTime;

        Vector2 offset = new Vector2(speed * Time.time, 0);
        mesh.sharedMaterial.SetTextureOffset("_MainTex", -offset);
    }
}
