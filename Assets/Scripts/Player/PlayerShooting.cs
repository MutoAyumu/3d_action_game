using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float _maxRange = 100f;
    string _name = "Prism";

    [SerializeField] RectTransform _areaImage;
    [SerializeField] RectTransform _targetImage;

    int _count;
    EnemyBase _currentFocusTarget;

    public List<EnemyBase> _enemies = new List<EnemyBase>();

    private void Awake()
    {
        CreateMesh();
    }

    private void Update()
    {
        TargetFocus();
    }

    /// <summary>
    /// 一番近い敵をフォーカスする
    /// </summary>
    void TargetFocus()
    {
        if (_enemies.Count > 0)
        {
            //リストの先頭をターゲットにする
            _currentFocusTarget = _enemies.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).FirstOrDefault();
        }
        else
        {
            //リストが空の間は、ターゲットをNullにする
            _currentFocusTarget = null;
        }

        if (_targetImage)
        {
            if (_currentFocusTarget)
            {
                //フォーカスイメージの座標ををターゲットの座標に書き換える
                _targetImage.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _currentFocusTarget.transform.position);
            }
        }
    }

    /// <summary>
    /// 索敵範囲を作成する
    /// </summary>
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

        var col = gameObject.AddComponent<MeshCollider>();
        col.convex = true;
        col.isTrigger = true;

        col.sharedMesh = mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (!_enemies.Contains(enemy)) //敵がリスト内に格納されていなければ追加
            {
                _enemies.Add(enemy);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (_enemies.Contains(enemy)) //敵がリスト内に格納されていれば削除
            {
                _enemies.Remove(enemy);
            }
        }
    }
}
