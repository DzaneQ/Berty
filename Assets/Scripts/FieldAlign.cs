using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldAlign
{
    MeshRenderer render;
    public FieldAlign(FieldGrid fieldGrid, MeshRenderer meshRender)
    {
        UpdateMeshMaterial(fieldGrid, meshRender);
    }

    public virtual void UpdateMeshMaterial(FieldGrid fieldGrid, MeshRenderer render)
    {
        render.material = fieldGrid.GetMaterial();
    }

    public virtual bool IsOccupied()
    {
        return false;
    }
    public virtual int IsPlayerField()
    {
        return 0;
    }
}
