using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBar : MonoBehaviour
{
    private CardSprite card;
    private Transform barFill;
    private SpriteRenderer rend;

    void Awake()
    {
        card = GetComponentInParent<CardSprite>();
        barFill = transform.GetChild(0);
        rend = barFill.GetComponent<SpriteRenderer>();
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

    private void MoveAndScaleBar(float widthDiff, Vector2 targetSize, AnimatingCard animate)
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
            rend.size = targetSize;
        }
        else
        {
            //Debug.Log($"Animating {name} for card {barFill.parent.parent.name} with target width: {targetSize.x}");
            NewBar properties = new(barFill, rend, targetPosition, targetSize);
            StartCoroutine(animate.MoveAndScaleBar(properties, 1f));
        }
    }

    public void UpdateBar(AnimatingCard animation)
    {
        Vector2 barSize = rend.size;
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
        MoveAndScaleBar(barSize.x - oldWidth, barSize, animation);
    }

    public void HideBar()
    {
        barFill.parent.gameObject.SetActive(false);
        //rend.enabled = false;
    }

    public void ShowBar()
    {
        barFill.parent.gameObject.SetActive(true);
        //rend.enabled = true;
    }
}
