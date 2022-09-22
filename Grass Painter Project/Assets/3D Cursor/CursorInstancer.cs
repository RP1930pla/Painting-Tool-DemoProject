using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CursorInstancer : MonoBehaviour
{
    private Vector3 HitNormalGiz;
    private Vector3 HitPosGiz;
    private int LayerUI;
    [Header("References")]
    public GameObject Gizmo;
    public Camera Cam;


    private void Start()
    {
        //LayerUI = LayerMask.NameToLayer("UI");
    }
    void Update()
    {
        RaycastHit hit;
        Ray Ray;

        Ray = Cam.ScreenPointToRay(Input.mousePosition);
        
        //Trans = new Matrix4x4[Density];
        
        if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition),out hit))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Collider coll = hit.collider;
                HitNormalGiz = hit.normal;
                HitPosGiz = hit.point;

                //Collider coll = hit.collider;
                //HitNormalGiz = hit.normal;
                //HitPosGiz = hit.point;

                if (coll != null)
                {
                    Gizmo.transform.position = HitPosGiz;
                    Gizmo.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }

            if(EventSystem.current.IsPointerOverGameObject() == true)
            {
                //Debug.Log("UIIII");
                Gizmo.transform.position = new Vector3(6.27f, 27.96f, -1.23f);
                Gizmo.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
        }

    }



}
