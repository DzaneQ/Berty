using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private FieldGrid fieldGrid;
    private CardSprite occupantCard;
    private MeshRenderer meshRender;
    private int[] coordinates = new int[2];
    private FieldAlign align;
    private FieldAlign Align
    {
        set
        {
            align = value;
            UpdateMeshMaterial();
        }
    }
    //public Alignment Align { get => align; }

    private void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        fieldGrid = GetComponentInParent<FieldGrid>();
        ConvertField(Alignment.None);
        //occupantCard = transform.GetChild(0).gameObject.GetComponent<CardSprite>();
        InitiateCardSprite();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == occupantCard.gameObject)
        {
            ConvertField(fieldGrid.Turn.CurrentAlignment);
        }
        else throw new Exception("Field colliding not with card");
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && fieldGrid.Turn.IsSelectionNow())
        {
            align.HandleLeftClick(this);
            //Debug.Log("Setting a card");
        }
    }

    private void InitiateCardSprite()
    {
        GameObject cardObject = Instantiate(fieldGrid.CardPrefab, new Vector3(0, 0, 0.001f), Quaternion.Euler(0, 180f, 180f));
        cardObject.transform.SetParent(transform, false);
        occupantCard = cardObject.GetComponent<CardSprite>();
    }

    private void UpdateMeshMaterial()
    {
        meshRender.material = fieldGrid.GetMaterial(align.Side);
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

    public CardSprite OccupantCard
    {
        get => occupantCard;
        set
        {
            occupantCard = value;
            occupantCard.OccupiedField = this;
            if (!value.gameObject.activeSelf) ConvertField(Alignment.None);
            else ConvertField(fieldGrid.Turn.CurrentAlignment);
        }
    }

    public int[] GetRelativeCoordinates(float angle = 0)
    {
        int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
        int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
        int[] relCoord = new int[2];
        relCoord[0] = cosinus * coordinates[0] - sinus * coordinates[1];
        relCoord[1] = sinus * coordinates[0] + cosinus * coordinates[1];
        //Debug.Log("Angle:" + angle + "; X: " + relCoord[0] + "; Y: " + relCoord[1]);
        return relCoord;
    }

    public void ConvertField(Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.None:
                Align = new FieldAlign();
                break;
            case Alignment.Player:
                Align = new PlayerField();
                break;
            case Alignment.Opponent:
                Align = new OpponentField();
                break;
        }
        //align = alignment;
        //UpdateMeshMaterial();
    }

    public void PlayCard()
    {
        occupantCard.ActivateCard();
    }

    public bool IsAligned(Alignment alignment)
    {
        return align.IsAligned(alignment);
    }


    public bool IsOccupied()
    {
        //Debug.Log("X=" + coordinates[0] + "; Y=" + coordinates[1] + "; isActive? " + occupantCard.gameObject.activeSelf);
        if (occupantCard.gameObject.activeSelf) return true;
        return false;
    }
}
