using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeSettings : MonoBehaviour
{
    [Header("Script References")]
    public CursorInstancer Cursor;
    public BatchGrassOptimized Grass;
    public BrushEngine Brush;
    public SkinnedMeshRenderer CursorMesh;
    public Material CursorMat;
    public Material CursorMat2;

    public Color32 ColorYes;
    public Color32 ColorNo;
    public Camera SceneCamera;
    [Header("Brush & Grass References")]    
    public Mesh GrassMesh;
    public Material GrassMaterial;
    public Texture2D BrushTexture;
    public float MaxTextureBrushSize;
    public float MinTextureBrushSize;
    public float MaxGrass = 2f;
    public float MinGrass = 0.1f;
    [Header("Runtime Settings")] 
    [Range(0, 1)] public float BrushSize;
    [Range(1, 6)] public int Instances;
    [Range(0, 1)] public float Normal_Mask;
    private RaycastHit hit;
    private int LayerPaint;
    private bool opensett;
    public GameObject Settings;
    
    void Start()
    {
        LayerPaint = LayerMask.NameToLayer("Paintable");
        opensett = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(SceneCamera.ScreenPointToRay(Input.mousePosition), out hit)  )
        {
            if (hit.transform.gameObject.layer == LayerPaint)
            {
                CursorMat.SetColor("_tint",ColorYes);
                CursorMat2.SetColor("_tint",ColorYes);
            }
            else
            {
                CursorMat.SetColor("_tint",ColorNo);
                CursorMat2.SetColor("_tint",ColorNo);
            }
        }

        
        Grass.BrushSize = Mathf.Lerp(MinGrass,MaxGrass,BrushSize);
        Brush.brushSize = Mathf.Lerp(MinTextureBrushSize, MaxTextureBrushSize, BrushSize);
        CursorMesh.SetBlendShapeWeight(0,BrushSize*100f);
        
        Brush.NormalLimit = Normal_Mask;
        Brush.brushTexture = BrushTexture;
        
        Grass.Density = Instances;
        Grass.normallimit = Normal_Mask;
        Grass.GrassMesh = GrassMesh;
        Grass.GrassMat = GrassMaterial;

        Grass.Cam = SceneCamera;
        Brush.Cam = SceneCamera;
        Cursor.Cam = SceneCamera;

    }

    public void ChangeSize(float size)
    {
        BrushSize = size;
    }

    public void ChangeN(float value)
    {
        Normal_Mask = value;
    }
    public void OpenSettings()
    {
        if (opensett)
        {
            Settings.SetActive(false);
            opensett = false;
            
        }
        else
        {
            Settings.SetActive(true);
            opensett = true;
        }
    }

    public void ResetAll()
    {
        SceneManager.LoadScene(0);
    }
    
}
