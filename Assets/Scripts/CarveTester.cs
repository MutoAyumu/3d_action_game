using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarveTester : MonoBehaviour
{
    private void FixedUpdate()
    {
        var hit = Physics.OverlapBox(this.transform.position, this.transform.localScale / 2);

        foreach (var go in hit)
        {
            var carve = go.GetComponent<CarveScript>();

            if (!carve) continue;

            carve.Carve(this.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, this.transform.localScale);
    }
}
