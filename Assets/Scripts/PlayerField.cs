using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerField : FieldAlign
{
    public PlayerField(FieldGrid fieldGrid, MeshRenderer meshRender) : base(fieldGrid, meshRender)
    {
        UpdateMeshMaterial(fieldGrid, meshRender);
    }
    public override void UpdateMeshMaterial(FieldGrid fieldGrid, MeshRenderer render)
    {
        render.material = fieldGrid.GetMaterial(1);
    }
    public override bool IsOccupied()
    {
        return true;
    }
    public override int IsPlayerField()
    {
        return 1;
    }
}
