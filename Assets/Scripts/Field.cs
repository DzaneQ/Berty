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
    //private Outline outline;
    private readonly int[] coordinates = new int[2];
    Alignment align;
    private bool underAttack = false;

    public FieldGrid Grid => fg;
    public CardSprite OccupantCard => occupantCard;
    //public CardSprite BackupCard => backupCard;
    public Alignment Align => align;
    public Renderer FieldRenderer => mr; // TODO: Delete when CustomCOS not used.
    //public Outline FieldOutline => outline;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        fg = GetComponentInParent<FieldGrid>();
        //outline = GetComponent<Outline>();
        //outline.enabled = false;
    }

    private void Start()
    {
        ConvertField(Alignment.None);
    }

    public void InstantiateCardSprite(GameObject prefab) // TODO: Change it!
    {
        occupantCard = Instantiate(prefab, transform).GetComponent<CardSprite>();
        occupantCard.name = $"Card ({GetX()}, {GetY()})"; //For debugging purposes.
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject == occupantCard.gameObject) UpdateMeshMaterial();
        //else throw new Exception($"Field colliding not with card {collision.gameObject}"); 
        if (collision.gameObject != occupantCard.gameObject) throw new Exception($"Field colliding not with card {collision.gameObject}");
    }

    private void OnMouseOver()
    {
        occupantCard.OnMouseOver();
    }

    private void UpdateMeshMaterial()
    {
        mr.material = fg.GetMaterial(align, underAttack);
    }

    //public void HighlightField()
    //{
    //    // TODO: Highlight this field!
    //    if (occupantCard.gameObject.activeSelf) occupantCard.HighlightCard();
    //}

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

    public void PlaceCard(CardSprite card, Alignment newAlign)
    {
        if (!card.gameObject.activeSelf)
        {
            occupantCard = card;
            card.SetField(this, false);
            ConvertField(newAlign);
        }
        else
        {
            StartCoroutine(MoveCardCoroutine(card, newAlign));
        }
    }

    public void PlaceBackupCard(CardSprite card)
    {
        backupCard = occupantCard;
        backupCard.SetIdle();
        card.DisableButtons();
        //card.transform.rotation = backupCard.transform.rotation; // TODO: Change rotation appropriately!
        occupantCard = card;
        card.SetField(this, false);
        card.RotateCard(Mathf.RoundToInt(card.transform.localEulerAngles.z - backupCard.transform.localEulerAngles.z));
        Debug.Log($"Rotacja karty {backupCard.name} po RotateCard: {occupantCard.transform.localEulerAngles.z}");
        card.LoadSelectedCard();
        Debug.Log($"Rotacja karty {backupCard.name} po LoadSelectedCard: {occupantCard.transform.localEulerAngles.z}");
    }

    private IEnumerator MoveCardCoroutine(CardSprite card, Alignment newAlign)
    {
        card.DisableButtons();
        yield return card.Animate.MoveToField(this, 1f);
        occupantCard = card;
        card.SetField(this, true);
        ConvertField(newAlign);
        card.EnableButtons();
        yield return null;
    }

    public void SynchronizeRotation()
    {
        if (!AreThereTwoCards()) throw new Exception("Synchronizing non-existent backup card!");
        occupantCard.transform.rotation = backupCard.transform.rotation;
        //backupCard.UpdateRelativeCoordinates();
    }

    /*public void SynchronizePosition()
    {
        if (backupCard == null) return;
        backupCard.UpdateRelativeCoordinates();
    }*/

    public bool AreThereTwoCards() => backupCard != null;

    public void AttachCards()
    {
        backupCard.transform.SetParent(occupantCard.transform, true);
        backupCard.HideBars();
    }
    
    private void DetachCards()
    {
        backupCard.transform.SetParent(transform, true);
        occupantCard = backupCard;
        backupCard = null;
        occupantCard.ShowBars();
        occupantCard.UpdateRelativeCoordinates();
    }

    public int[] GetRelativeCoordinates(float angle = 0) // TODO: Merge
    {
        return Grid.GetRelativeCoordinates(GetX(), GetY(), -angle);
    }

    public void ConvertField(Alignment alignment)
    {
        align = alignment;
        UpdateMeshMaterial();
    }

    public void HighlightField(Color color)
    {
        underAttack = true;
        if (OccupantCard.gameObject.activeSelf) OccupantCard.HighlightCard(color);
        else UpdateMeshMaterial();
    }

    public void UnhighlightField()
    {
        //if (underAttack) Debug.Log("Unhighlighting field...");
        if (!underAttack) return;
        underAttack = false;
        if (OccupantCard.gameObject.activeSelf) OccupantCard.UnhighlightCard();
        else UpdateMeshMaterial();
    }

    public void PlayCard()
    {
        occupantCard.LoadSelectedCard();
    }

    public void AdjustCardRemoval()
    {
        if (!AreThereTwoCards()) ConvertField(Alignment.None);
        else DetachCards();
    }

    public void TransferBackupCard(Field destination)
    {
        destination.SetBackupCard(backupCard);
        backupCard = null;
    }

    public void SetBackupCard(CardSprite card)
    {
        backupCard = card;
        backupCard.ApplyField(this);
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
}
