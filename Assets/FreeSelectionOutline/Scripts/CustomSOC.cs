using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;

/*
 *  Not compatible with URP and HDRP at this moment.
    This function requires 3 full-screen-size rendertextures,and the outline shader contains for-loop.
    The cost of this is acceptable on PC,but if you are gonna use it on mobile platforms, you'd better optimize this by yourself.
*/
//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CustomSOC : MonoBehaviour
{
    /*public enum AlphaType : int
    {
        KeepHoles = 0,
        Intact = 1
    }*/
    public enum OutlineMode : int
    {
        Whole = 0,
        ColorizeOccluded = 1,
        OnlyVisible = 2
    }
    private Material OutlineMat;
    private Shader OutlineShader, TargetShader;
    private RenderTexture Mask, Outline;
    private Camera cam;
    private CommandBuffer cmd;
    private bool Ini = false, Selected = false;
    [Tooltip("The last two type will require rendering an extra Camera Depth Texture.")]
    public OutlineMode OutlineType = OutlineMode.ColorizeOccluded;
    //[Tooltip("Decide whether the alpha data of the main texture affect the outline.")]
    //public AlphaType AlphaMode = AlphaType.KeepHoles;
    private Renderer TargetRenderer, lastTarget;
    [ColorUsageAttribute(true, true)]
    public Color OutlineColor = new Color(1f, 0.55f, 0f, 1f), OccludedColor = new Color(0.5f, 0.9f, 0.3f, 1f);
    [Range(0, 1)]
    public float OutlineWidth = 0.1f;
    [Range(0, 1)]
    public float OutlineHardness = 0.85f;

    // Custom
    private Field[] fields;
    private Renderer[] fieldRenders;

    void OnEnable()
    {
        //Inital();
        AssignFields();
    }
    private void AssignFields()
    {
        FieldGrid fg = (FieldGrid)FindFirstObjectByType(typeof(FieldGrid));
        fields = new Field[9];
        fieldRenders = new Renderer[9];
        if (fields.Length != fg.transform.childCount) Debug.LogError("Number of fields is not equal to number of child count!");
        for (int index = 0; index < fields.Length; index++)
        {
            fields[index] = fg.transform.GetChild(index).GetComponent<Field>();
            fieldRenders[index] = fields[index].GetComponent<Renderer>();
        }
    }
    void Inital()
    {
#if UNITY_WEBGL
        Shader.EnableKeyword("_WEBGL");
#endif
        OutlineShader = Shader.Find("Outline/PostprocessOutline");
        TargetShader = Shader.Find("Outline/Target");
        if(OutlineShader==null||TargetShader==null)
        {
            Debug.LogError("Can't find the outline shaders,please check the Always Included Shaders in Graphics settings.");
            return;
        }
        cam = GetComponent<Camera>();
        cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.None : DepthTextureMode.Depth;
        OutlineMat = new Material(OutlineShader);
        if (OutlineType > 0)
        {
            Shader.EnableKeyword("_COLORIZE");
            Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RFloat);
            Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RG16);
            if (OutlineType == OutlineMode.OnlyVisible)
                Shader.EnableKeyword("_OCCLUDED");
            else
                Shader.DisableKeyword("_OCCLUDED");

        }
        else
        {
            Shader.DisableKeyword("_OCCLUDED");
            Shader.DisableKeyword("_COLORIZE");
            Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
            Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
        }
        cam.RemoveAllCommandBuffers();
        cmd = new CommandBuffer { name = "Outline Command Buffer" };
        cmd.SetRenderTarget(Mask);
        cam.AddCommandBuffer(CameraEvent.BeforeImageEffects, cmd);
        FieldGrid fg = (FieldGrid)FindFirstObjectByType(typeof(FieldGrid));
        fields = fg.Fields;
        if (fields == null)
        {
            Debug.LogWarning("Fields not loaded yet.");
            return;
        }
        Ini = true;
    }
    private void OnValidate()
    {
        if (!Ini) Inital();
        cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.Depth : DepthTextureMode.None;
        if (OutlineType > 0)
        {
            Shader.EnableKeyword("_COLORIZE");

            if (OutlineType == OutlineMode.OnlyVisible)
                Shader.EnableKeyword("_OCCLUDED");
            else
                Shader.DisableKeyword("_OCCLUDED");

        }
        else
        {
            Shader.DisableKeyword("_OCCLUDED");
            Shader.DisableKeyword("_COLORIZE");
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (OutlineMat == null)
        {
            Inital();
            if(!Ini)
            return;
        }
        OutlineMat.SetFloat("_OutlineWidth", OutlineWidth * 10f);
        OutlineMat.SetFloat("_OutlineHardness", 8.99f * (1f - OutlineHardness) + 0.01f);
        OutlineMat.SetColor("_OutlineColor", OutlineColor);
        OutlineMat.SetColor("_OccludedColor", OccludedColor);

        OutlineMat.SetTexture("_Mask", Mask);
        Graphics.Blit(source, Outline, OutlineMat, 0);
        OutlineMat.SetTexture("_Outline", Outline);
        Graphics.Blit(source, destination, OutlineMat, 1);
        //Graphics.Blit(Outline, destination);

    }
    void RenderTarget(Renderer target, bool blockState = false)
    {
        Material TargetMat = new Material(TargetShader);
        /*bool MainTexFlag = false;
        string[] attrs = target.sharedMaterial.GetTexturePropertyNames();
        foreach (var c in attrs)
        {
            if (c == "_MainTex")
            {
                MainTexFlag = true;
                break;
            }
        }
        if (MainTexFlag && target.sharedMaterial.mainTexture != null && AlphaMode == AlphaType.KeepHoles)
        {
            TargetMat.mainTexture = target.sharedMaterial.mainTexture;
        }*/
        cmd.DrawRenderer(target, TargetMat);
        Graphics.ExecuteCommandBuffer(cmd);
    }
    void SetTarget()
    {
        cmd.SetRenderTarget(Mask);
        cmd.ClearRenderTarget(true, true, Color.black);
        Selected = true;
        if (TargetRenderer != null) RenderTarget(TargetRenderer);
        else Debug.LogWarning("No renderer provided for outline.");
    }
    void SetTargets(int sourceIndex)
    {
        cmd.SetRenderTarget(Mask);
        cmd.ClearRenderTarget(true, true, Color.black);
        Selected = true;
        CardSprite sourceCard = fields[sourceIndex].OccupantCard;
        if (TargetRenderer != null)
        {
            bool riposte = false;
            foreach (int[] distance in sourceCard.Character.AttackRange)
            {
                Field targetField = sourceCard.GetTargetField(distance);
                if (targetField == null) continue;
                bool block = false;
                if (targetField.IsOccupied())
                {
                    CardSprite targetCard = targetField.OccupantCard;
                    if (!riposte &&
                        targetCard.Character.CanRiposte(targetCard.GetFieldDistance(fields[sourceIndex]))) riposte = true;
                    if (targetCard.Character.CanBlock(targetCard.GetFieldDistance(fields[sourceIndex]))) block = true;
                }
                int targetIndex = Array.IndexOf(fields, targetField);
                if (targetIndex < 0) throw new Exception("Target index not found! It's " + targetIndex);
                RenderTarget(fieldRenders[targetIndex], block);
            }
            if (riposte) RenderTarget(fieldRenders[sourceIndex]);
        }
        else Debug.LogWarning("No renderer provided for outline.");
    }
    void ClearTarget()
    {
        Selected = false;
        cmd.ClearRenderTarget(true, true, Color.black);

        Graphics.ExecuteCommandBuffer(cmd);
        cmd.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int fieldIndex;
        if (Physics.Raycast(ray, out hit) && IsValidField(hit, out fieldIndex))
        {
            //Debug.Log("Hit: " + hit.transform.gameObject.name);
            //TargetRenderer = hit.transform.GetComponent<Renderer>();
            TargetRenderer = fieldRenders[fieldIndex];
            if (lastTarget == null) lastTarget = TargetRenderer;

            if (TargetRenderer != lastTarget || !Selected)
            {
                //SetTarget();
                SetTargets(fieldIndex);
            }
            //Debug.DrawRay(transform.position, hit.point - transform.position, Color.blue);
            lastTarget = TargetRenderer;
        }
        else
        {
            TargetRenderer = null;
            lastTarget = null;
            if (Selected) ClearTarget();
        }
    }

    private bool IsValidField(RaycastHit hit, out int index)
    {
        GameObject targetObject = hit.transform.gameObject;
        //if (fields == null) return false;
        index = -1;
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].gameObject == targetObject || fields[i].OccupantCard.gameObject == targetObject)
            {
                if (fields[i].OccupantCard.gameObject.activeSelf)
                {
                    index = i;
                    return true;
                }
                else return false;
            }
        }
        return false;
    }
}
