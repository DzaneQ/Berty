using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBar : MonoBehaviour
{
    private CardSprite card;
    private SpriteRenderer rend;

    void Awake()
    {
        card = GetComponentInParent<CardSprite>();
        rend = GetComponent<SpriteRenderer>();
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

    private void MoveBar(float value)
    {
        switch (name)
        {
            case "StrengthBar":
                transform.localPosition += new Vector3(-value, 0, 0);
                break;

            case "PowerBar":
                transform.localPosition += new Vector3(0, value, 0);
                break;

            case "DexterityBar":
                transform.localPosition += new Vector3(value, 0, 0);
                break;

            case "HealthBar":
                transform.localPosition += new Vector3(0, -value, 0);
                break;

            default:
                throw new System.Exception("Unknown stat for bar: " + name);
        }
    }

    public void UpdateBar()
    {
        float oldWidth = rend.size.x;
        float newWidth;  
        
        int stat = ReadStat();
        float startOutlineWidth = .33f;
        float unitWidth = 2.66f;
        float maxWidth = 16.36f;
        switch (stat)
        {
            case 0:
                newWidth = 0;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                newWidth = startOutlineWidth + (unitWidth * stat);
                break;
            case 6:
                newWidth = maxWidth;
                break;
            default:
                throw new System.Exception("Wrong stat number: " + stat);
        }
        rend.size = new Vector2(newWidth, rend.size.y);
        MoveBar((oldWidth - newWidth) / 2);
    }
}
