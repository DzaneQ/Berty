using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentField : FieldAlign
{

    public OpponentField()
    {
        alignment = Alignment.Opponent;
    }
    //public OpponentField(FieldGrid fieldGrid, MeshRenderer meshRender) : base(fieldGrid, meshRender)
    //{
    //    alignment = Alignment.Opponent;
    //    UpdateMeshMaterial(fieldGrid, meshRender);
    //}
    //public override void UpdateMeshMaterial(FieldGrid fieldGrid, MeshRenderer render)
    //{
    //    render.material = fieldGrid.GetMaterial(Alignment.Opponent);
    //}

    //public override bool IsOccupied()
    //{
    //    return true;
    //}
    //public override int IsPlayerField()
    //{
    //    return -1;
    //}
    public override void HandleLeftClick(Field field) { }
}
