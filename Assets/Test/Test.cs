
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Header("�쐬����J�����̐ݒ�")]
    [SerializeField] float _fov = 45f;
    [SerializeField] float _maxRange = 100f;
    [SerializeField, Tooltip("���C���J������Depth��菬�����l")] int _depth = -2;

    string _name = "Prism";

    [SerializeField] Canvas _canvas;

    [SerializeField] RectTransform _areaImage;

    //ToDo
    //�~�X�N���[�����W���烁�b�V������鎞�̍��W���Ƃ�
    //�摜�̃T�C�Y���r���[�|�[�g�ɕϊ����Ă�������[���h�ϊ�

    private void Start()
    {
        CreateMesh();
    }

    Camera CreateCamera()
    {
        //�V�����J�����̍쐬
        var go = new GameObject();
        var cam = go.AddComponent<Camera>();

        //�����ݒ�
        cam.transform.SetParent(Camera.main.transform);
        cam.gameObject.name = _name;
        cam.depth = _depth;
        cam.fieldOfView = _fov;

        return cam;
    }

    private void CreateMesh()
    {
        var cam = CreateCamera();

        Vector3[] vertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

        var rect = _areaImage.GetComponent<RectTransform>();
        var width = rect.sizeDelta.x / 2;
        var higth = rect.sizeDelta.y / 2;

        for (int i = 0; i < 8; i++)
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
}