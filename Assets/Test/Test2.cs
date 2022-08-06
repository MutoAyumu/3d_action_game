using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    [SerializeField] float _maxRange = 100f;
    string _name = "Prism";

    [SerializeField] RectTransform _areaImage;

    [SerializeField] Material _t1, _t2;

    [SerializeField] LayerMask _layer;

    private void Start()
    {
        CreateMesh();
    }

    private void CreateMesh()
    {
        if (!_areaImage) return;

        var rect = _areaImage.GetComponent<RectTransform>();
        var width = rect.sizeDelta.x / 2;
        var higth = rect.sizeDelta.y / 2;

        var cam = Camera.main;
        var Delta = cam.ScreenToViewportPoint(new Vector3(width, higth, 1));


        Vector3[] vertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0.5f - Delta.x, 0.5f + Delta.y, 1),
            new Vector3 (0.5f + Delta.x, 0.5f + Delta.y, 1),
            new Vector3 (0.5f + Delta.x, 0.5f - Delta.y, 1),
            new Vector3 (0.5f - Delta.x, 0.5f - Delta.y, 1),
        };

        for (int i = 0; i < 8; i++)
        {
            var vec = vertices[i];

            if (i > 3)
            {
                vec.z += _maxRange;
            }

            vec = cam.ViewportToWorldPoint(vec);

            vertices[i] = vec - cam.transform.position;
        }

        int[] triangles = {
            0, 2, 1,
            0, 3, 2,
            2, 3, 4,
            2, 4, 5,
            1, 2, 5,
            1, 5, 6,
            0, 7, 4,
            0, 4, 3,
            5, 4, 7,
            5, 7, 6,
            0, 6, 7,
            0, 1, 6
        };

        Mesh mesh = new Mesh();
        mesh.name = _name;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();

        //var go = new GameObject();

        //go.transform.position = Camera.main.transform.position;
        //go.transform.SetParent(cam.transform);
        //go.name = _name;

        //var col = go.gameObject.AddComponent<MeshCollider>();
        var col = gameObject.AddComponent<MeshCollider>();
        col.convex = true;
        col.isTrigger = true;

        col.sharedMesh = mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        var renderer = other.GetComponent<MeshRenderer>();

        if(renderer && other.gameObject.layer == 7)
        {
            renderer.material = _t1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var renderer = other.GetComponent<MeshRenderer>();

        if (renderer && other.gameObject.layer == 7)
        {
            renderer.material = _t2;
        }
    }
}
