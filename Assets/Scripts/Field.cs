using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    //[SerializeField] protected Material defaultMaterial;
    //[SerializeField] protected Material player;
    //[SerializeField] protected Material opponent;
    //[SerializeField] protected GameObject cardPrefab;

    private bool isFree; //TO BE GONE!
    private bool isPlayerField; //TO BE GONE!
    private FieldGrid fieldGrid;
    //private TurnManager turn;
    //private Rigidbody cardRB;
    private CardSprite occupantCard;
    //private Vector3 defaultCardPosition;
    //private Quaternion defaultCardRotation;
    private MeshRenderer meshRender;
    private int[] coordinates = new int[2];
    private FieldAlign align;

    //public FieldGrid _fieldGrid { get => fieldGrid; }
    //public CardSprite _occupantCard { get => occupantCard; }
    //public MeshRenderer _meshRender { get => meshRender;  }
    //public int[] _coordinates { get => coordinates; }

    ////public virtual Field(Field data)
    ////{
    ////    ImportData(data);
    ////}

    //protected void ImportData(Field data)
    //{
    //    this.fieldGrid = data._fieldGrid;
    //    this.occupantCard = data._occupantCard;
    //    this.meshRender = data._meshRender;
    //    this.coordinates = data._coordinates;
    //}

    private void Start()
    {
        fieldGrid = GetComponentInParent<FieldGrid>();
        meshRender = GetComponent<MeshRenderer>();
        align = new FieldAlign(fieldGrid, meshRender);
        //defaultCardPosition = new Vector3(0, 0, 0.001f);
        //defaultCardRotation = Quaternion.Euler(0, 180f, 180f);
        //SetDefaultCardTransformValues();
        PrepareCardSprite();
        isFree = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        UpdateAlignment(fieldGrid.Turn().GetCurrentAlignment());
        //UpdateMeshMaterial();
        occupantCard.UnlockButtons();
        //if (collision.gameObject.GetComponent<CardSprite>() != null)
        //    occupantCard = collision.gameObject.GetComponent<CardSprite>(); // how to replace GetComponent?
        //if (cardRB != null) Destroy(cardRB);
        //cardRB.isKinematic = true;
    }

    private void UpdateAlignment(TurnManager.Alignment currentAlign)
    {
        if (currentAlign == TurnManager.Alignment.Player) align = new PlayerField(fieldGrid, meshRender);
        else align = new OpponentField(fieldGrid, meshRender);
        UpdateMeshMaterial();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && isFree && fieldGrid.Turn().IsSelectionNow())
        {
            PlayCard();
            //Debug.Log("Setting a card");
        }
        //GetRelativeCoordinates(0);
        //GetRelativeCoordinates(90);
        //GetRelativeCoordinates(180);
        //GetRelativeCoordinates(270);
    }

    //private void SetDefaultCardTransformValues()
    //{
    //    defaultCardPosition = new Vector3(0, 0, 0.001f);
    //    defaultCardRotation = Quaternion.Euler(0, 180f, 180f);
    //}

    //public void SetCardToDefaultTransform()
    //{
    //    //if (defaultCardPosition == null || defaultCardRotation == null) SetDefaultCardTransformValues();
    //    occupantCard.transform.localPosition = defaultCardPosition;
    //    occupantCard.transform.localRotation = defaultCardRotation;
    //}

    //private void UpdateMeshMaterial()
    //{
    //    if (!occupantCard.gameObject.activeSelf) mesh.material = defaultMaterial;
    //    else if (turn.IsPlayerTurn() == 1)
    //    {
    //        mesh.material = player;
    //        isPlayerField = true;
    //        //Debug.Log("Field updated.");
    //    }
    //    else
    //    {
    //        mesh.material = opponent;
    //        isPlayerField = false;
    //    }
    //}

    private void UpdateMeshMaterial()
    {
        align.UpdateMeshMaterial(fieldGrid, meshRender);
    }

    //public void AttachGrid(FieldGrid grid)
    //{
    //    fieldGrid = grid;
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

    //public int[] GetCoordinates()
    //{
    //    return coordinates;
    //}

    public CardSprite GetOccupantCard()
    {
        return occupantCard;
    }

    public void SetOccupantCard(CardSprite value)
    {
        occupantCard = value;
        if (value.gameObject.activeSelf) isFree = false;
        else isFree = true;
        UpdateMeshMaterial();
        fieldGrid.AdjustCards();
    }

    public int[] GetRelativeCoordinates(float angle = 0)
    {
        int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
        int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
        //int direction = (int)Math.Round(angle / 90) % 4;
        int[] relCoord = new int[2];
        relCoord[0] = cosinus * coordinates[0] - sinus * coordinates[1];
        relCoord[1] = sinus * coordinates[0] + cosinus * coordinates[1];
        //Debug.Log("Angle:" + angle + "; X: " + relCoord[0] + "; Y: " + relCoord[1]);
        return relCoord;
    }

    public void BecomeNeutral()
    {
        align = new FieldAlign(fieldGrid, meshRender);
        meshRender.material = fieldGrid.GetMaterial();
        //occupantCard = null;
        isFree = true;
    }

    private void PrepareCardSprite()
    {
        GameObject cardObject = Instantiate(fieldGrid.GetCardSprite(), new Vector3(0, 0, 0.001f), Quaternion.Euler(0, 180f, 180f));
        cardObject.transform.SetParent(transform, false);
        occupantCard = cardObject.GetComponent<CardSprite>();
        //cardRB = cardObject.GetComponent<Rigidbody>();
        //cardRB.detectCollisions = true;
    }

    private void PlayCard()
    {
        //GameObject cardObject = transform.GetChild(0).gameObject;
        occupantCard.ActivateCard();
        //cardRB = cardObject.AddComponent<Rigidbody>();
        //cardRB.detectCollisions = true;
        isFree = false;
        fieldGrid.AdjustCards();
    }

    public bool IsOccupied()
    {
        //if (isFree) return false;
        //return true;
        return align.IsOccupied();
    }

    public int IsPlayerField()
    {
        //Debug.Log("Is free?" + isFree);
        //if (isFree) return 0;
        //if (isPlayerField) return 1;
        //return -1;
        return align.IsPlayerField();
    }
}
