using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// プレイヤーに射撃をさせるクラス
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Vector2 _size;
    [SerializeField] float _maxDistance = 100;
    [SerializeField] LayerMask _mask;

	Camera _mainCamera;

    private void Awake()
    {
		CreateMesh();
		_mainCamera = Camera.main;
    }

    private void Update()
    {
		this.transform.rotation = _mainCamera.transform.rotation;
    }
    void CreateMesh()
    {
		var t = this.transform.position;
		var halfX = _size.x / 2 + t.x;
		var halfY = _size.y / 2 + t.y;

		Vector3[] vertices = {
			new Vector3 (-halfX, -halfY, t.z),
			new Vector3 (halfX, -halfY, t.z),
			new Vector3 (halfX, halfY, t.z),
			new Vector3 (-halfX, halfY, t.z),
			new Vector3 (-halfX, halfY, _maxDistance),
			new Vector3 (halfX, halfY, _maxDistance),
			new Vector3 (halfX, -halfY, _maxDistance),
			new Vector3 (-halfX, -halfY, _maxDistance),
		};

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

		var col = this.gameObject.AddComponent<MeshCollider>();

		//メッシュの作成・最適化
		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize();
		mesh.RecalculateNormals();

		//コライダーの初期化
		col.convex = true;
		col.isTrigger = true;

		col.sharedMesh = mesh;
	}
}
