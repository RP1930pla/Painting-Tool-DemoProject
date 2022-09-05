using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushEngine : MonoBehaviour
{
    // Texture CONFIG //
    [Header("Texture Settings")]
    public int TextureResolution = 512;
    public Texture2D whiteMap;
    public Camera Cam;

    // Brush CONFIG //
    [Header("Brush Settings")]
    public float brushSize;
    public Texture2D brushTexture;
    public float NormalLimit = 1;

    // Ray & Render Texture Info //
    Vector2 PreviousRay;
    private int LayerPaint;
    public static Dictionary<Collider, RenderTexture> paintTEX = new Dictionary<Collider, RenderTexture>();

    private void Start()
    {
        ClearTexture();
        LayerPaint = LayerMask.NameToLayer("Paintable");
        Debug.Log("START");
    }
    private void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit) && hit.normal.y <=(1+NormalLimit) && hit.normal.y >=(1-NormalLimit))
        {
            Collider coll = hit.collider;
            if (coll != null)
            {
                if (Input.GetMouseButton(0) || Input.touchCount==1)
                {
                    if (hit.transform.gameObject.layer == LayerPaint)
                    {
                        // if there is already paint on the material, add to that //
                        if (!paintTEX.ContainsKey(coll))
                        {
                            // Get the renderer of the hit //
                            Renderer rend = hit.transform.GetComponent<Renderer>();
                            // Add key to the dictionary //
                            paintTEX.Add(coll, getWhiteRenderTex());
                            // Set the texture related to the object to the material //
                            rend.material.SetTexture("_PaintMap", paintTEX[coll]);
                        }

                        if (PreviousRay != hit.lightmapCoord) // Check if it's not the same point as the frame before //
                        {
                            // Store Previous Ray //
                            PreviousRay = hit.lightmapCoord;
                            Vector2 pixelUV = hit.lightmapCoord;
                            // Convert 0-1 to 0-TextureResolution //
                            pixelUV.y *= TextureResolution;
                            pixelUV.x *= TextureResolution;
                            StartCoroutine(DelayExecution(pixelUV.x, pixelUV.y, coll));
                            //DrawOnTexture(paintTEX[coll], pixelUV.x, pixelUV.y);
                        }

                    }
                    else
                    {
                        StopAllCoroutines();
                    }
                }
            }
        }

    }


    IEnumerator DelayExecution(float pixelX, float pixelY, Collider coll)
    {
        yield return new WaitForSeconds(0.03f);
        DrawOnTexture(paintTEX[coll], pixelX,pixelY);
    }
    
    
    RenderTexture getWhiteRenderTex()
    {
        // Create RenderTexture with the desired texture resolution and color depht //
        RenderTexture rt = new RenderTexture(TextureResolution, TextureResolution, 32);
        // Copy Blank 1x1 White texture to the new Render Texture //
        Graphics.Blit(whiteMap, rt);
        return rt;
        

    }

    // Creates a new texture that is 1x1 pixels with the single pixel being white //
    void ClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        //whiteMap.Reinitialize(TextureResolution, TextureResolution);
        whiteMap.Apply();

    }

    void DrawOnTexture(RenderTexture rt, float posX, float posY)
    {
        RenderTexture.active = rt; // Activate the use of the provided RenderTexture //
        //GL.Flush();
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, TextureResolution, TextureResolution, 0);
        
        // Draw using the brush texture //
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null; // Turn off RenderTexture //
    }
}
