
using UnityEngine;

public class Test : MonoBehaviour
{
	[Header("作成するカメラの設定")]
    [SerializeField] float _fov = 45f;
    [SerializeField] float _maxRange = 100f;
    [SerializeField] float _minRange = 100f;
    [SerializeField] float _aspect = 1.78f;
    [SerializeField, Tooltip("メインカメラのDepthより小さい値")] int _depth = -2;

    string _name = "Prism";

    [SerializeField] Canvas _canvas;
    readonly float _planeDistance = 1;

    [SerializeField] RectTransform _areaImage;

    [Header("メッシュの各頂点")]
	[SerializeField, Clamp01Vector] Vector2 _frontTopRight = new Vector2(1, 1);
	[SerializeField, Clamp01Vector] Vector2 _frontTopLeft = new Vector2(0, 1);
	[SerializeField, Clamp01Vector] Vector2 _frontDownRight = new Vector2(1, 0);
	[SerializeField, Clamp01Vector] Vector2 _frontDownLeft = new Vector2(0, 0);
	[SerializeField, Clamp01Vector] Vector2 _backTopRight = new Vector2(1, 1);
	[SerializeField, Clamp01Vector] Vector2 _backTopLeft = new Vector2(0, 1);
	[SerializeField, Clamp01Vector] Vector2 _backDownRight = new Vector2(1, 0);
	[SerializeField, Clamp01Vector] Vector2 _backDownLeft = new Vector2(0, 0);


	private void Start()
    {
        CreateMesh();
    }

    void SetupCanvas(Camera cam)
    {
        if (!_canvas) return;

        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = cam;
        _canvas.planeDistance = _planeDistance;
    }

    Camera CreateCamera()
    {
        var go = new GameObject();
        var cam = go.AddComponent<Camera>();

        cam.transform.SetParent(Camera.main.transform);
        cam.gameObject.name = _name;
        cam.depth = _depth;
        cam.fieldOfView = _fov;

        return cam;
    }

    private void CreateMesh()
    {
        var cam = CreateCamera();

        SetupCanvas(cam);

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
            var vec = vertices[i];

            if (i > 3)
            {
                vec.z += _maxRange;
            }

            vec = cam.ViewportToWorldPoint(vec);

            vertices[i] = vec;
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

        cam.transform.position = Camera.main.transform.position;

        var col = cam.gameObject.AddComponent<MeshCollider>();
        col.convex = true;
        col.isTrigger = true;

        col.sharedMesh = mesh;
    }

    private void Update()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 center = Camera.main.transform.position;

        var cache = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, Camera.main.transform.rotation, transform.lossyScale);

        //錐台を描画
        Gizmos.DrawFrustum(Vector3.zero, _fov, _maxRange, _minRange, _aspect);

        Gizmos.matrix = cache;
    }
}