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

	//bool _isSearch;
	Camera _mainCamera;

    private void Awake()
    {
		CreateMesh();
		_mainCamera = Camera.main;
    }

    private void Update()
    {
		//_isSearch = SearchArea();
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

    //bool SearchArea()
    //{
    //    var center = this.transform.position;
    //    var halfSize = new Vector3(_size.x, _size.y, _maxDistance) * 0.5f;

    //    var area = Physics.BoxCastAll(center, halfSize, this.transform.forward, Camera.main.transform.rotation, _maxDistance * 0.5f, _mask);

    //    if (area.Length <= 0)
    //        return false;

    //    return true;
    //}
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = _isSearch ? Color.green : Color.red;

    //    Gizmos.matrix = Matrix4x4.TRS(this.transform.position, Camera.main.transform.rotation, transform.localScale);

    //    var size = new Vector3(_size.x, _size.y, _maxDistance);

    //    Gizmos.DrawWireCube(Vector3.forward * _maxDistance * 0.5f, size);
    //}
}
