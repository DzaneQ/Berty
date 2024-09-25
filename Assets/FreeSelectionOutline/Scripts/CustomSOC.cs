//using System.Collections;
//using System.Collections.Generic;
//using System;
//using UnityEngine;
//using UnityEngine.Rendering;

///*
// *  Not compatible with URP and HDRP at this moment.
//    This function requires 3 full-screen-size rendertextures,and the outline shader contains for-loop.
//    The cost of this is acceptable on PC,but if you are gonna use it on mobile platforms, you'd better optimize this by yourself.
//*/
//[ExecuteInEditMode]
//[RequireComponent(typeof(Camera))]
//public class CustomSOC : MonoBehaviour
//{
//    /*public enum AlphaType : int
//    {
//        KeepHoles = 0,
//        Intact = 1
//    }*/
//    public enum OutlineMode : int
//    {
//        Whole = 0,
//        ColorizeOccluded = 1,
//        OnlyVisible = 2
//    }
//    private Material OutlineAttMat;
//    private Material OutlineBckMat;
//    private Shader OutlineShader, TargetShader;
//    private RenderTexture Mask, Outline, Source, Dest;
//    private Camera cam;
//    private CommandBuffer cmd;
//    private bool Ini = false;//, Selected = false;
//    [Tooltip("The last two type will require rendering an extra Camera Depth Texture.")]
//    public OutlineMode OutlineType = OutlineMode.ColorizeOccluded;
//    //[Tooltip("Decide whether the alpha data of the main texture affect the outline.")]
//    //public AlphaType AlphaMode = AlphaType.KeepHoles;
//    private Renderer TargetRenderer, lastTarget;
//    //[ColorUsageAttribute(true, true)]
//    //public Color OutlineColor = new Color(1f, 0.55f, 0f, 1f);
//    public Color OccludedColor = new Color(0.5f, 0.9f, 0.3f, 1f);
//    private Color AttackColor = new Color(1f, 0.55f, 0f, 1f);
//    private Color BlockColor = new Color(0f, 0.35f, 0.8f, 1f);
//    [Range(0, 1)]
//    public float OutlineWidth = 0.1f;
//    [Range(0, 1)]
//    public float OutlineHardness = 0.85f;

//    // Custom
//    private Field[] fields;
//    private CardSprite selectedCard;

//    void Awake()
//    {
//        //Inital();
//        AssignFields();
//    }

//    private void AssignFields()
//    {
//        FieldGrid fg = (FieldGrid)FindFirstObjectByType(typeof(FieldGrid));
//        fields = new Field[9];
//        if (fields.Length != fg.transform.childCount) Debug.LogError("Number of fields is not equal to number of child count!");
//        for (int index = 0; index < fields.Length; index++) fields[index] = fg.transform.GetChild(index).GetComponent<Field>();
//    }

//    void Inital()
//    {
//        //Debug.Log("SOC: Inital");
//#if UNITY_WEBGL
//        Shader.EnableKeyword("_WEBGL");
//#endif
//        OutlineShader = Shader.Find("Outline/PostprocessOutline");
//        TargetShader = Shader.Find("Outline/Target");
//        if(OutlineShader==null||TargetShader==null)
//        {
//            Debug.LogError("Can't find the outline shaders,please check the Always Included Shaders in Graphics settings.");
//            return;
//        }
//        cam = GetComponent<Camera>();
//        cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.None : DepthTextureMode.Depth;
//        OutlineAttMat = new Material(OutlineShader);
//        OutlineBckMat = new Material(OutlineShader);
//        if (OutlineType > 0)
//        {
//            Shader.EnableKeyword("_COLORIZE");
//            Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RFloat);
//            Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RG16);
//            if (OutlineType == OutlineMode.OnlyVisible)
//                Shader.EnableKeyword("_OCCLUDED");
//            else
//                Shader.DisableKeyword("_OCCLUDED");

//        }
//        else
//        {
//            Shader.DisableKeyword("_OCCLUDED");
//            Shader.DisableKeyword("_COLORIZE");
//            Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
//            Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
//        }
//        cam.RemoveAllCommandBuffers();
//        cmd = new CommandBuffer { name = "Outline Command Buffer" };
//        cmd.SetRenderTarget(Mask);
//        cam.AddCommandBuffer(CameraEvent.BeforeImageEffects, cmd);
//        FieldGrid fg = (FieldGrid)FindFirstObjectByType(typeof(FieldGrid));
//        fields = fg.Fields;
//        if (fields != null) Ini = true;
//    }

//    private void OnValidate()
//    {
//        //Debug.Log("SOC: On validate");
//        if (!Ini) Inital();
//        cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.Depth : DepthTextureMode.None;
//        if (OutlineType > 0)
//        {
//            Shader.EnableKeyword("_COLORIZE");

//            if (OutlineType == OutlineMode.OnlyVisible)
//                Shader.EnableKeyword("_OCCLUDED");
//            else
//                Shader.DisableKeyword("_OCCLUDED");

//        }
//        else
//        {
//            Shader.DisableKeyword("_OCCLUDED");
//            Shader.DisableKeyword("_COLORIZE");
//        }
//    }
//    private void OnRenderImage(RenderTexture source, RenderTexture destination)
//    {
//        //Debug.Log("SOC: OnRenderImage");
//        if (Ini) return;
//        if (OutlineAttMat == null)
//        {
//            Inital();
//            if(!Ini)
//            {
//                Debug.LogError("Initialization failed!");
//                return;
//            }
//        }
//        Source = source;
//        Dest = destination;

//        OutlineAttMat.SetFloat("_OutlineWidth", OutlineWidth * 10f);
//        OutlineAttMat.SetFloat("_OutlineHardness", 8.99f * (1f - OutlineHardness) + 0.01f);
//        OutlineAttMat.SetColor("_OutlineColor", AttackColor);
//        OutlineAttMat.SetColor("_OccludedColor", OccludedColor);

//        OutlineBckMat.SetFloat("_OutlineWidth", OutlineWidth * 10f);
//        OutlineBckMat.SetFloat("_OutlineHardness", 8.99f * (1f - OutlineHardness) + 0.01f);
//        OutlineBckMat.SetColor("_OutlineColor", BlockColor);
//        OutlineBckMat.SetColor("_OccludedColor", OccludedColor);

//        OutlineAttMat.SetTexture("_Mask", Mask);
//        OutlineBckMat.SetTexture("_Mask", Mask);
//        OutlineAttMat.SetTexture("_Outline", Outline);
//        OutlineBckMat.SetTexture("_Outline", Outline);
//        Graphics.Blit(Source, Outline, OutlineAttMat, 0);
//        Graphics.Blit(Source, Dest, OutlineAttMat, 1);
//        //Graphics.Blit(Outline, destination);
//    }

//    void RenderTarget(Renderer target, bool blockState = false)
//    {
//        //if (!blockState)
//        //{
//        //    Graphics.Blit(Source, Outline, OutlineBckMat, 0);
//        //    Graphics.Blit(Source, Dest, OutlineBckMat, 1);
//        //}
//        //Material TargetMat = new Material(TargetShader);
//        Material TargetMat = blockState ? OutlineBckMat : OutlineBckMat;
//        cmd.DrawRenderer(target, TargetMat);
//        Graphics.ExecuteCommandBuffer(cmd);
//        //if (!blockState)
//        //{
//        //    Graphics.Blit(Source, Outline, OutlineAttMat, 0);
//        //    Graphics.Blit(Source, Dest, OutlineAttMat, 1);
//        //}
//    }

//    void SetTargets(int sourceIndex)
//    {
//        //cmd.Clear();  // TODO: Understand this command.
//        cmd.SetRenderTarget(Mask);
//        cmd.ClearRenderTarget(true, true, Color.black); 
//        if (fields[sourceIndex].OccupantCard == selectedCard) return;
//        if (selectedCard != null) selectedCard.DisableButtons();
//        selectedCard = fields[sourceIndex].OccupantCard;
//        selectedCard.EnableButtons();
//        if (TargetRenderer != null)
//        {
//            bool riposte = false;
//            foreach (int[] distance in selectedCard.Character.AttackRange)
//            {
//                Field targetField = selectedCard.GetTargetField(distance);
//                if (targetField == null) continue;
//                bool block = false;
//                if (targetField.IsOccupied())
//                {
//                    CardSprite targetCard = targetField.OccupantCard;
//                    if (!riposte &&
//                        targetCard.Character.CanRiposte(targetCard.GetFieldDistance(fields[sourceIndex]))) riposte = true;
//                    if (targetCard.Character.CanBlock(targetCard.GetFieldDistance(fields[sourceIndex]))) block = true;
//                }
//                int targetIndex = Array.IndexOf(fields, targetField);
//                if (targetIndex < 0) throw new Exception("Target index not found! It's " + targetIndex);
//                RenderTarget(fields[targetIndex].FieldRenderer, block);
//            }
//            if (riposte) RenderTarget(fields[sourceIndex].FieldRenderer);
//        }
//        else Debug.LogWarning("No renderer provided for outline.");
//    }

//    void ClearTarget()
//    {
//        //Selected = false;
//        selectedCard.DisableButtons();
//        selectedCard = null;
//        cmd.ClearRenderTarget(true, true, Color.black);

//        Graphics.ExecuteCommandBuffer(cmd);
//        cmd.Clear();
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;
//        int fieldIndex;
//        if (Physics.Raycast(ray, out hit) && IsValidField(hit, out fieldIndex))
//        {
//            TargetRenderer = fields[fieldIndex].FieldRenderer;
//            if (lastTarget == null) lastTarget = TargetRenderer;
//            if (TargetRenderer != lastTarget || selectedCard == null) SetTargets(fieldIndex);
//            //Debug.DrawRay(transform.position, hit.point - transform.position, Color.blue);
//            lastTarget = TargetRenderer;
//        }
//        else
//        {
//            if (TargetRenderer == null)
//            {
//                if (lastTarget != null) Debug.LogError("Last target not null with target renderer being null");
//                return;
//            }
//            TargetRenderer = null;
//            lastTarget = null;
//            ClearTarget();
//        }
//    }

//    private bool IsValidField(RaycastHit hit, out int index)
//    {
//        Transform targetObject = hit.transform;
//        index = -1;
//        for (int i = 0; i < fields.Length; i++)
//        {
//            if (IsFieldTargeted(fields[i], targetObject))
//            {
//                if (fields[i].OccupantCard.gameObject.activeSelf)
//                {
//                    index = i;
//                    return true;
//                }
//                else return false;
//            }
//        }
//        return false;
//    }

//    private bool IsFieldTargeted(Field field, Transform targetTransform)
//    {
//        if (field.transform == targetTransform) return true;
//        if (field.OccupantCard.transform == targetTransform) return true;
//        if (targetTransform.parent != null && targetTransform.parent.parent != null 
//            && field.OccupantCard.transform == targetTransform.parent.parent) return true;
//        return false;
//    }
//}
