using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerField : FieldAlign
{

    public PlayerField()
    {
        alignment = Alignment.Player;
    }

    //public PlayerField(FieldGrid fieldGrid, MeshRenderer meshRender) : base(fieldGrid, meshRender)
    //{
    //    alignment = Alignment.Player;
    //    UpdateMeshMaterial(fieldGrid, meshRender);
    //}
    //public override void UpdateMeshMaterial(FieldGrid fieldGrid, MeshRenderer render)
    //{
    //    render.material = fieldGrid.GetMaterial(Alignment.Player);
    //}

    //public override bool IsOccupied()
    //{
    //    return true;
    //}
    //public override int IsPlayerField()
    //{
    //    return 1;
    //}
    public override void HandleLeftClick(Field field) { }
}
