using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoard : MonoBehaviour
{
    [SerializeField] float _power = 1f;
    [SerializeField] float _lifeTime = 1.5f;

    Rigidbody _rb;
    Transform _thisTransform;

    public void OnSetup(Rigidbody rigidbody, Vector3 vector)
    {
        _rb = rigidbody;
        _rb.velocity = Vector3.zero;
        _thisTransform = this.transform;

        if (vector == Vector3.zero)
        {
            _thisTransform.Rotate(270, 0, 0);
        }
        else
        {
            _thisTransform.LookAt(vector);
            _thisTransform.Rotate(340, 0, 0);
        }

        Bounce();

        Destroy(this.gameObject, _lifeTime);
    }

    void Bounce()
    {
        _rb.AddForce(_thisTransform.forward * _power, ForceMode.VelocityChange);
    }
}
