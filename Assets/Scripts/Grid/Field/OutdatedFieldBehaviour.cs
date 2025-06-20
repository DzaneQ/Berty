using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Field
{
    public class OutdatedFieldBehaviour : MonoBehaviour
    {
        private FieldGrid fg;
        private CardSpriteBehaviour occupantCard;
        private CardSpriteBehaviour backupCard;
        private MeshRenderer mr;
        //private Outline outline;
        private readonly int[] coordinates = new int[2]; // TODO: Entity property - remove
        Alignment align; // TODO: Entity property - remove
        private bool underAttack = false;

        public FieldGrid Grid => fg;
        public CardSpriteBehaviour OccupantCard => occupantCard; // TODO: Entity property?
        //public CardSprite BackupCard => backupCard;
        public Alignment Align => align; // TODO: Entity property - remove
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

        public void InstantiateCardSprite(GameObject prefab)
        {
            occupantCard = Instantiate(prefab, transform).GetComponent<CardSpriteBehaviour>();
            occupantCard.name = $"Card ({GetX()}, {GetY()})"; //For debugging purposes.
        }

        public void SetCoordinates(int x, int y)
        {
            coordinates[0] = x;
            coordinates[1] = y;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject == occupantCard.gameObject) UpdateMeshMaterial();
            //else throw new Exception($"Field colliding not with card {collision.gameObject}"); 
            if (collision.gameObject != occupantCard.gameObject) throw new Exception($"Field colliding not with card {collision.gameObject}");
        }

        private void UpdateMeshMaterial()
        {
            mr.material = fg.GetMaterial(align, underAttack);
        }

        public int GetX()
        {
            return coordinates[0];
        }

        public int GetY()
        {
            return coordinates[1];
        }

        public void PlaceCard(CardSpriteBehaviour card, Alignment newAlign)
        {
            if (!card.gameObject.activeSelf)
            {
                occupantCard = card;
                card.SetField(this, false);
                ConvertField(newAlign);
            }
            else
            {
                ConvertField(Alignment.None);
                StartCoroutine(MoveCardCoroutine(card, newAlign));
            }
        }

        public void PlaceBackupCard(CardSpriteBehaviour card)
        {
            backupCard = occupantCard;
            backupCard.SetIdle();
            card.DisableButtons();
            occupantCard = card;
            card.SetField(this, false);
            card.RotateCard(Mathf.RoundToInt(card.transform.localEulerAngles.z - backupCard.transform.localEulerAngles.z));
            Debug.Log($"Rotacja karty {backupCard.name} po RotateCard: {occupantCard.transform.localEulerAngles.z}");
            card.LoadSelectedCard();
            Debug.Log($"Rotacja karty {backupCard.name} po LoadSelectedCard: {occupantCard.transform.localEulerAngles.z}");
        }

        private IEnumerator MoveCardCoroutine(CardSpriteBehaviour card, Alignment newAlign)
        {
            card.DisableButtons();
            fg.Turn.DisableInteractions(false);
            yield return card.Animate.MoveToField(this, 1f);
            occupantCard = card;
            card.SetField(this, true);
            ConvertField(newAlign);
            card.EnableButtons();
            if (!card.IsAnimating()) fg.Turn.EnableInteractions();
            yield return null;
        }

        public void SynchronizeRotation()
        {
            if (!AreThereTwoCards()) throw new Exception("Synchronizing non-existent backup card!");
            occupantCard.transform.rotation = backupCard.transform.rotation;
            //backupCard.UpdateRelativeCoordinates();
        }

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

        public int[] GetRelativeCoordinates(float angle = 0)
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

        public void TransferBackupCard(OutdatedFieldBehaviour destination)
        {
            destination.SetBackupCard(backupCard);
            backupCard = null;
        }

        public void SetBackupCard(CardSpriteBehaviour card)
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
}