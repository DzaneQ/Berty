using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private FieldGrid fg;
    private CardSprite occupantCard;
    private MeshRenderer meshRender;
    private readonly int[] coordinates = new int[2];
    Alignment align;

    public FieldGrid Grid => fg;
    public CardSprite OccupantCard => occupantCard;
    public Alignment Align => align;

    private void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();  
    }

    private void Start()
    {
        fg = GetComponentInParent<FieldGrid>();
        ConvertField(Alignment.None);
    }

    public void InstantiateCardSprite(GameObject prefab) // TODO: Change it!
    {
        occupantCard = Instantiate(prefab, transform).GetComponent<CardSprite>();
        occupantCard.name = $"Card ({GetX()}, {GetY()})"; //For debugging purposes.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == occupantCard.gameObject) UpdateMeshMaterial();
        else throw new Exception($"Field colliding not with card {collision.gameObject}"); 
    }

    private void OnMouseDown()
    {
        occupantCard.OnMouseDown();
    }

    private void UpdateMeshMaterial()
    {
        meshRender.material = fg.GetMaterial(align);
    }

    public void SetCoordinates(int x, int y)
    {
        coordinates[0] = x;
        coordinates[1] = y;
    }

    public int GetX()
    {
        return coordinates[0];
    }

    public int GetY()
    {
        return coordinates[1];
    }

    public void TakeCard(CardSprite card)
    {
        occupantCard = card;
        occupantCard.SetField(this);
    }

    public int[] GetRelativeCoordinates(float angle = 0) // TODO: Merge
    {
        //int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
        //int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
        //int[] relCoord = new int[2];
        //relCoord[0] = cosinus * coordinates[0] - sinus * coordinates[1];
        //relCoord[1] = sinus * coordinates[0] + cosinus * coordinates[1];
        //Grid.GetRelativeField(coordinates[0], coordinates[1], angle)
        return Grid.GetRelativeCoordinates(GetX(), GetY(), -angle);
    }

    public void ConvertField(Alignment alignment, bool colorize = true)
    {
        align = alignment;
        if (colorize) UpdateMeshMaterial();
    }

    public void PlayCard()
    {
        occupantCard.TryToActivateCard();
    }

    public bool IsAligned(Alignment alignment)
    {
        return align == alignment;
    }

    public bool IsOccupied()
    {
        if (align != Alignment.None) return true;
        //if (occupantCard.gameObject.activeSelf) return true;
        return false;
    }

    public void GoToOppositeSide()
    {
        if (align == Alignment.Player) ConvertField(Alignment.Opponent);
        else if (align == Alignment.Opponent) ConvertField(Alignment.Player);
        else throw new Exception($"Can't switch sides for field {name}");
    }
}
