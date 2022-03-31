using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldAlign
{
    protected MeshRenderer render;
    protected Alignment alignment;
    public Alignment Side { get => alignment; }

    public FieldAlign()
    {
        alignment = Alignment.None;
    }

    //public FieldAlign(FieldGrid fieldGrid, MeshRenderer meshRender)
    //{
    //    alignment = Alignment.None;
    //    UpdateMeshMaterial(fieldGrid, meshRender);
    //}

    public void UpdateMeshMaterial(FieldGrid fieldGrid, MeshRenderer render)
    {
        render.material = fieldGrid.GetMaterial(alignment);
    }

    public bool IsAligned(Alignment align)
    {
        if (align == alignment) return true;
        else return false;
    }

    //public virtual bool IsOccupied()
    //{
    //    return false;
    //}
    //public virtual int IsPlayerField()
    //{
    //    return 0;
    //}

    public virtual void HandleLeftClick(Field field)
    {
        field.PlayCard();
    }
}
