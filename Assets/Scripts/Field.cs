using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private FieldGrid fg;
    private CardSprite occupantCard;
    private CardSprite backupCard;
    private MeshRenderer mr;
    private readonly int[] coordinates = new int[2];
    Alignment align;

    public FieldGrid Grid => fg;
    public CardSprite OccupantCard => occupantCard;
    public Alignment Align => align;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();  
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

    private void OnMouseOver()
    {
        occupantCard.OnMouseOver();
    }

    private void UpdateMeshMaterial()
    {
        mr.material = fg.GetMaterial(align);
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

    public void PlaceCard(CardSprite card, bool setBackup = false)
    {
        if (setBackup)
        {
            backupCard = occupantCard;
            backupCard.SetIdle();
            card.DisableButtons();
            card.transform.rotation = backupCard.transform.rotation;
        }
        occupantCard = card;
        occupantCard.SetField(this);
    }

    public void SynchronizeRotation()
    {
        if (backupCard == null) return; 
        backupCard.transform.rotation = occupantCard.transform.rotation;
        backupCard.UpdateRelativeCoordinates();
    }

    public void SynchronizePosition()
    {
        if (backupCard == null) return;
        backupCard.UpdateRelativeCoordinates();
    }

    public int[] GetRelativeCoordinates(float angle = 0) // TODO: Merge
    {
        return Grid.GetRelativeCoordinates(GetX(), GetY(), -angle);
    }

    public void ConvertField(Alignment alignment, bool colorize = true)
    {
        align = alignment;
        if (colorize) UpdateMeshMaterial();
    }

    public void PlayCard()
    {
        occupantCard.LoadSelectedCard();
    }

    public void AdjustCardRemoval()
    {
        if (backupCard == null) ConvertField(Alignment.None);
        else
        {
            occupantCard = backupCard;
            backupCard = null;
        }
    }

    public void TransferBackupCard(Field destination)
    {
        destination.SetBackupCard(backupCard);
        backupCard = null;
    }

    public void SetBackupCard(CardSprite card)
    {
        backupCard = card;
        backupCard.SetField(this);
    }

    public bool IsAligned(Alignment alignment)
    {
        return align == alignment;
    }

    public bool IsOpposed(Alignment alignment)
    {
        if (alignment == Alignment.None) return false;
        if (align == Alignment.None) return false;
        return align != alignment;
    }

    public bool IsOccupied()
    {
        if (align != Alignment.None) return true;
        return false;
    }

    public void GoToOppositeSide()
    {
        if (align == Alignment.Player) ConvertField(Alignment.Opponent);
        else if (align == Alignment.Opponent) ConvertField(Alignment.Player);
        else throw new Exception($"Can't switch sides for field {name}");
    }

    public bool AreThereTwoCards() => backupCard != null;
}
