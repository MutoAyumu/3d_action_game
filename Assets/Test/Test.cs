
using UnityEngine;

public class Test : MonoBehaviour
{
	[Header("Gizmoで描画する推台の設定")]
    [SerializeField] float fov = 45f;
    [SerializeField] float maxRange = 100f;
    [SerializeField] float minRange = 0.3f;
    [SerializeField] float aspect = 1.78f;

    [SerializeField] MeshCollider _collider;

	[Header("メッシュの各頂点")]
	[SerializeField] Vector3 _frontTopRight = new Vector3(1, 1, 0);
	[SerializeField] Vector3 _frontTopLeft = new Vector3(0, 1, 0);
	[SerializeField] Vector3 _frontDownRight = new Vector3(1, 0, 0);
	[SerializeField] Vector3 _frontDownLeft = new Vector3(0, 0, 0);
	[SerializeField] Vector3 _backTopRight = new Vector3(1, 1, 1);
	[SerializeField] Vector3 _backTopLeft = new Vector3(0, 1, 1);
	[SerializeField] Vector3 _backDownRight = new Vector3(1, 0, 1);
	[SerializeField] Vector3 _backDownLeft = new Vector3(0, 0, 1);


	private void Start()
    {
        CreateMesh();
    }

    private void CreateMesh()
    {
        var center = Camera.main.transform.position;

        Vector3[] vertices = {
            _frontDownLeft,
            _frontDownRight,
            _frontTopRight,
            _frontTopLeft,
            _backTopLeft,
            _backTopRight,
            _backDownRight,
            _backDownLeft,
        };

        for(int i = 0; i < 8; i++)
        {
            vertices[i] += center;
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
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();

        _collider.sharedMesh = mesh;
    }

    private void Update()
    {

    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;

    //    Vector3 center = Camera.main.transform.position;

    //    var cache = Gizmos.matrix;
    //    Gizmos.matrix = Matrix4x4.TRS(center, Camera.main.transform.rotation, transform.lossyScale);

    //    //錐台を描画
    //    Gizmos.DrawFrustum(Vector3.zero, fov, maxRange, minRange, aspect);

    //    Gizmos.matrix = cache;
    //}
}