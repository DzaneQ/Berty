using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBar : MonoBehaviour
{
    private CardSprite card;
    private Transform barFill;
    private SpriteRenderer fillRend;
    private SpriteRenderer outlineRend;

    void Awake()
    {
        card = GetComponentInParent<CardSprite>();
        barFill = transform.GetChild(0);
        fillRend = barFill.GetComponent<SpriteRenderer>();
        outlineRend = barFill.parent.GetComponent<SpriteRenderer>();
        //Debug.Log("Max width value: " + maxWidth);
    }

    private int ReadStat()
    {
        switch (name)
        {
            case "StrengthBar":
                return card.CardStatus.Strength;

            case "PowerBar":
                return card.CardStatus.Power;

            case "DexterityBar":
                return card.CardStatus.Dexterity;

            case "HealthBar":
                return card.CardStatus.Health;

            default:
                throw new System.Exception("Unknown stat for bar: " + name);
        }
    }

    private IEnumerator MoveAndScaleBar(float widthDiff, Vector2 targetSize, AnimatingCardSprite animate)
    {
        Vector3 targetPosition = barFill.localPosition;
        targetPosition.x += widthDiff / 2;
        /*switch (name)
        {
            case "StrengthBar":
                targetPosition.x += widthDiff / 2;
                break;

            case "PowerBar":
                targetPosition.y += -widthDiff / 2;
                break;

            case "DexterityBar":
                targetPosition.x += -widthDiff / 2;
                break;

            case "HealthBar":
                targetPosition.y += widthDiff / 2;
                break;

            default:
                throw new System.Exception("Unknown stat for bar: " + name);
        }*/
        if (animate == null)
        {
            barFill.localPosition = targetPosition;
            fillRend.size = targetSize;
            yield return null;
        }
        else
        {
            //Debug.Log($"Animating {name} for card {barFill.parent.parent.name} with target width: {targetSize.x}");
            NewBar properties = new(barFill, fillRend, targetPosition, targetSize);
            card.Grid.Turn.DisableInteractions(false);
            yield return StartCoroutine(animate.MoveAndScaleBar(properties, 1f));
            if (!card.IsAnimating()) card.Grid.Turn.EnableInteractions();
            yield return null;
        }
    }

    public IEnumerator UpdateBar(AnimatingCardSprite animation)
    {
        Vector2 barSize = fillRend.size;
        float oldWidth = barSize.x;
        //float newWidth;
        
        int stat = ReadStat();
        float startOutlineWidth = .165f;
        float unitWidth = 2.66f;
        //float maxWidth = 16.36f;
        barSize.x = startOutlineWidth + (unitWidth * stat);
        /*switch (stat)
        {
            case 0:
                barSize.x = 0;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                barSize.x = startOutlineWidth + (unitWidth * stat);
                break;
            case 6:
                barSize.x = maxWidth;
                break;
            default:
                throw new System.Exception("Wrong stat number: " + stat);
        }*/
        //Debug.Log($"Checking {name}: {stat} for card {barFill.parent.parent.name}. Changing width from {oldWidth} to {barSize.x}");
        yield return StartCoroutine(MoveAndScaleBar(barSize.x - oldWidth, barSize, animation));
        if (stat > 0) yield break;
        if (name == "PowerBar") card.ZeroPowerAdjustment();
        else if (name == "HealthBar") card.ZeroHealthAdjustment();
        yield return null;
    }

    public void HideBar()
    {
        //barFill.parent.gameObject.SetActive(false);
        fillRend.enabled = false;
        outlineRend.enabled = false;
    }

    public void ShowBar()
    {
        //barFill.parent.gameObject.SetActive(true);
        fillRend.enabled = true;
        outlineRend.enabled = true;
    }
}
