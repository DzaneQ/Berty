using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    private Turn turn;
    private CardManager cm;
    private FieldGrid fg;
    private Transform collection;

    [SerializeField] private GameObject targetCard;
    [SerializeField] private GameObject targetParent;
    [SerializeField] private Alignment targetAlignment;
    [SerializeField] private int cardRotation;

    void Start()
    {
        if (!Debug.isDebugBuild) Destroy(this);
        else
        {
            collection = GameObject.Find("/CardImageCollection").transform;
        }
    }

    public void Initialize(Turn turn, CardManager cm, FieldGrid fg)
    {
        this.turn = turn;
        this.cm = cm;
        this.fg = fg;
    }

    public void SetCardParent()
    {
        // Important note: when removing sprite from a field, make sure it's THE sprite from THIS field.
        // Common mistakes: 1. IMAGE instead of SPRITE. 2. Choosing THE sprite from THE PREVIOUS field.
        if (!turn.IsItMoveTime())
        {
            Debug.LogWarning("Use this during move time!");
            return;
        }
        CardImage image = targetCard.GetComponent<CardImage>();
        CardSprite sprite = targetCard.GetComponent<CardSprite>();
        Field field = targetParent.GetComponent<Field>();
        cm.DebugForceRemoveCardFromLists(image);
        if (image == null) image = sprite.DebugGetReference();
        fg.DebugForceRemoveCardFromField(sprite);
        switch (targetParent.name)
        {
            case "PlayerPosition":
                image.transform.SetParent(targetParent.transform, false);
                if (turn.CurrentAlignment == Alignment.Player) cm.DebugAssignCardToList(image, cm.EnabledCards);
                else cm.DebugAssignCardToList(image, cm.DisabledCards);
                break;
            case "OpponentPosition":
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
                    field.OccupantCard.DebugForceActivateCard(image, cardRotation * 90);
                    break;
                }
        }
    }

    public Field OpponentPriorityField()
    {
        Field field = targetParent.GetComponent<Field>();
        if (field == null || field.IsOccupied()) return null;
        return field;
    }
}
