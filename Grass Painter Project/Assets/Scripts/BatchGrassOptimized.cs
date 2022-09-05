using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


// OBJECT DATA CLASS (STORE INSTANCE INFO) //
public class ObjData
{
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos,rot,scale);
        }
    }

    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}


public class BatchGrassOptimized : MonoBehaviour
{
    [Header("Grass Config")]
    public int MaxInstances;
    public Mesh GrassMesh;
    public Material GrassMat;
    public Camera Cam;
    public float normallimit = 1f;

    [Header("Brush Settings")] 
    public int Density;
    [Range(0.1f,2f)]
    public float BrushSize;

    private Vector3 lastpos;
    private List<List<ObjData>> batches = new List<List<ObjData>>();
    private int batchIndexNum = 0;
    List<ObjData> currBatch = new List<ObjData>();
    private int numbatch;
    private Vector2 storedmouse;
    private int LayerPaint;
    private MaterialPropertyBlock block;

    private void Start()
    {
        batchIndexNum = 0;
        numbatch = 0;
        float frames = 30;
        float time = 1 / frames;
        LayerPaint = LayerMask.NameToLayer("Paintable");
    }


    private void RenderBatches()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(GrassMesh, 0, GrassMat, batch.Select((a) => a.matrix).ToList(), block,ShadowCastingMode.Off);
        }
    }

    private void Update()
    {
        RenderBatches();
        if (Input.GetMouseButton(0))
        {
            InvokeRepeating("DrawOnRay",0f,0.3f);
        }
        else
        {
            CancelInvoke();
        }
        //Debug.Log(batchIndexNum);
        //StartCoroutine(DrawCoroutine());
    }

    void DrawOnRay()
    {
        for (int q = 0; q < Density; q++) {
                // Offset Vector //
                Vector3 r_origin = Vector3.zero;
                // Circunference of a circle with a random radius based on brush size //
                float off = 2f * Mathf.PI * Random.Range(0f, BrushSize);
                float u = Random.Range(0f, BrushSize) + Random.Range(0f, BrushSize);
                // is radius greater than 1? then subtract 2 to the value // //ELSE u = u //
                float r = (u > 1 ? 2 - u : u);


                // Add randomness on every instance except the first //
                if (q != 0)
                {
                    r_origin.x += r * Mathf.Sin(off);
                    r_origin.y += r * Mathf.Cos(off);
                }
                else
                {
                    r_origin.x += r * Mathf.Sin(off);
                    r_origin.y += r * Mathf.Cos(off);
                    //r_origin = Vector3.zero;
                }


                Ray InstanceRay = Cam.ScreenPointToRay(Input.mousePosition);
                // Add offset to ray //
                InstanceRay.origin += r_origin;
                RaycastHit InsHit;
                if (Vector2.Distance(Input.mousePosition, storedmouse) > 0.1)
                {
                    if (Physics.Raycast(InstanceRay, out InsHit) &&
                        Vector3.Distance(InsHit.point, lastpos) > BrushSize && InsHit.normal.y <= (1 + normallimit) &&
                        InsHit.normal.y >= (1 - normallimit))
                    {
                        if (InsHit.transform.gameObject.layer == LayerPaint)
                        {
                            storedmouse = Input.mousePosition;
                            Vector3 Position = new Vector3(InsHit.point.x, InsHit.point.y, InsHit.point.z);
                            //Debug.Log(Position);
                            Quaternion Rot = Quaternion.FromToRotation(Vector3.up, InsHit.normal);
                            Vector3 Scale = new Vector3(Random.Range(0.3f,.5f), Random.Range(0.3f,0.5f), Random.Range(0.3f,0.5f));
                            AddObj(currBatch, Position, Scale, Rot);
                            //Debug.Log("DDD");
                            batchIndexNum++;

                            if (numbatch == 0)
                            {
                                batches.Add(currBatch);
                                currBatch = BuildNewBatch();
                                numbatch++;
                                batchIndexNum = 0;
                            }

                            if (batchIndexNum >= 30)
                            {
                                batches.Add(currBatch);
                                currBatch = BuildNewBatch();
                                batchIndexNum = 0;
                            }
                        }
                    }

                    lastpos = InsHit.point;
                }
        }
    }

    

    private List<ObjData> BuildNewBatch()
    {
        return new List<ObjData>();
    }

    private void AddObj(List<ObjData> currBatch, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        currBatch.Add(new ObjData(position,scale,rotation));
    }

    
    
    
    
}
