using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Material dfMaterial;
    [SerializeField] private Material player;
    [SerializeField] private Material opponent;
    [SerializeField] private GameObject cardPrefab;

    private bool isFree = true;
    private FieldGrid fieldGrid;
    private TurnManager turn;
    private Rigidbody cardRB;
    private CardSprite occupantCard;
    private MeshRenderer mesh;

    //private int column;
    //private int row;

    //public Field(int x, int y)
    //{
    //    column = x;
    //    row = y;
    //}
    private void Start()
    {
        fieldGrid = GetComponentInParent<FieldGrid>();
        turn = GameObject.Find("EventSystem").GetComponent<TurnManager>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (turn.IsPlayerTurn()) mesh.material = player;
        else mesh.material = opponent;
        if (collision.gameObject.GetComponent<CardSprite>() != null)
            occupantCard = collision.gameObject.GetComponent<CardSprite>(); // how to replace GetComponent?
        if (cardRB != null) Destroy(cardRB);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && isFree && turn.IsSelectionNow())
        {
            PutCard();
        }
    }

    public void AttachGrid(FieldGrid grid)
    {
        fieldGrid = grid;
    }

    public void BecomeNeutral()
    {
        mesh.material = dfMaterial;
        occupantCard = null;
        isFree = true;
    }

    private void PutCard()
    {
        GameObject selectedCard = Instantiate(cardPrefab, new Vector3(0, 0, 0.001f), Quaternion.Euler(0, 180f, 180f));
        selectedCard.transform.SetParent(transform, false);
        cardRB = selectedCard.AddComponent<Rigidbody>();
        cardRB.detectCollisions = true;
        isFree = false;
    }
}
