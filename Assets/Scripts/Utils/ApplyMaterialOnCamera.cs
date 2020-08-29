using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ApplyMaterialOnCamera : MonoBehaviour
{
    public Material matToApply;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (matToApply != null)
        {
            Graphics.Blit(source, destination, matToApply);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
