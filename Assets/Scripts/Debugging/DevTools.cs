using Berty.AutomaticPlayer;
using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;
using Berty.Grid;
using Berty.Gameplay;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Debugging
{
    public class DevTools : MonoBehaviour
    {
        private Turn turn;
        private OutdatedCardManager cm;
        private FieldGrid fg;
        private Transform collection;

        [SerializeField] private GameObject targetCard;
        [SerializeField] private GameObject targetParent;
        [SerializeField] private Alignment targetAlignment;
        [SerializeField] private int rightAngleCount;

        void Start()
        {
            if (!Debug.isDebugBuild) Destroy(gameObject);
            else
            {
                GameObject sys = GameObject.Find("/EventSystem");
                turn = sys.GetComponent<Turn>();
                cm = sys.GetComponent<OutdatedCardManager>();
                fg = GameObject.Find("/GameBoard/FieldBoard").GetComponent<FieldGrid>();
                sys.GetComponent<OpponentControl>().DebugInit(this);
                collection = GameObject.Find("/CardImageCollection").transform;
            }
        }

        /*public void Initialize(Turn turn, CardManager cm, FieldGrid fg)
        {
            this.turn = turn;
            this.cm = cm;
            this.fg = fg;
        }*/

        public void SetCardParent()
        {
            // Important note: when removing sprite from a field, make sure it's THE sprite from THIS field.
            // Common mistakes: 1. IMAGE instead of SPRITE. 2. Choosing THE sprite from THE PREVIOUS field.
            if (!turn.IsItMoveTime())
            {
                Debug.LogWarning("Use this during move time!");
                return;
            }
            if (targetCard == null || targetParent == null) return;
            HandCardBehaviour image = targetCard.GetComponent<HandCardBehaviour>();
            CardSpriteBehaviour sprite = targetCard.GetComponent<CardSpriteBehaviour>();
            OutdatedFieldBehaviour field = targetParent.GetComponent<OutdatedFieldBehaviour>();
            cm.DebugForceRemoveCardFromLists(image);
            if (image == null) image = sprite.DebugGetReference();
            fg.DebugForceRemoveCardFromField(sprite);
            switch (targetParent.name)
            {
                case "PlayerSide":
                    image.transform.SetParent(targetParent.transform, false);
                    if (turn.CurrentAlignment == Alignment.Player) cm.DebugAssignCardToList(image, cm.EnabledCards);
                    else cm.DebugAssignCardToList(image, cm.DisabledCards);
                    break;
                case "OpponentSide":
                    image.transform.SetParent(targetParent.transform, false);
                    if (turn.CurrentAlignment == Alignment.Player) cm.DebugAssignCardToList(image, cm.DisabledCards);
                    else cm.DebugAssignCardToList(image, cm.EnabledCards);
                    break;
                case "DrawPile":
                    image.transform.SetParent(collection, false);
                    cm.DebugAddPileCard(image);
                    break;
                case "DiscardPile":
                    image.transform.SetParent(collection, false);
                    cm.DebugDiscardPileCard(image);
                    break;
                case "DeadScreen":
                    cm.KillCard(image);
                    break;
                default:
                    if (field != null && targetAlignment == Alignment.None)
                    {
                        Debug.LogError("targetAlignment not set!");
                        break;
                    }
                    else if (field == null)
                    {
                        Debug.LogError("targetParent not found!");
                        break;
                    }
                    else
                    {
                        field.ConvertField(targetAlignment);
                        field.OccupantCard.DebugForceActivateCard(image, rightAngleCount * 90);
                        break;
                    }
            }
        }

        public OutdatedFieldBehaviour OpponentPriorityField()
        {
            if (targetParent == null) return null;
            OutdatedFieldBehaviour field = targetParent.GetComponent<OutdatedFieldBehaviour>();
            if (field == null || field.IsOccupied()) return null;
            return field;
        }
    }
}