using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorInstancer : MonoBehaviour
{
    private Vector3 HitNormalGiz;
    private Vector3 HitPosGiz;
    
    [Header("References")]
    public GameObject Gizmo;
    public Camera Cam;

    void Update()
    {
        RaycastHit hit;
        Ray Ray;

        Ray = Cam.ScreenPointToRay(Input.mousePosition);
        
        //Trans = new Matrix4x4[Density];
        
        if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition),out hit))
        {
            Collider coll = hit.collider;
            HitNormalGiz = hit.normal;
            HitPosGiz = hit.point;

            if (coll != null)
            {
                Gizmo.transform.position = HitPosGiz;
                Gizmo.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }

    }
}
