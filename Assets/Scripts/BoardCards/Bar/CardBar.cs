using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using UnityEngine;

namespace Berty.BoardCards.Bar
{
    public class CardBar : MonoBehaviour
    {
        const float startOutlineWidth = .165f;
        const float unitWidth = 2.66f;

        private BoardCardBehaviour card;
        private IBarWidth barWidth;
        private Transform barFill;
        private SpriteRenderer fillRend;
        private SpriteRenderer outlineRend;

        void Awake()
        {
            card = GetComponentInParent<BoardCardBehaviour>();
            barFill = transform.GetChild(0);
            barWidth = barFill.GetComponent<IBarWidth>();
            fillRend = barFill.GetComponent<SpriteRenderer>();
            outlineRend = barFill.parent.GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            UpdateBarWithoutAnimation();
        }

        private int ReadStat()
        {
            switch (name)
            {
                case "StrengthBar":
                    return card.BoardCard.Stats.Strength;

                case "PowerBar":
                    return card.BoardCard.Stats.Power;

                case "DexterityBar":
                    return card.BoardCard.Stats.Dexterity;

                case "HealthBar":
                    return card.BoardCard.Stats.Health;

                default:
                    throw new System.Exception("Unknown stat for bar: " + name);
            }
        }

        public void UpdateBar()
        {
            Vector2 barSize = fillRend.size;
            float oldWidth = barSize.x;
            int stat = ReadStat();
            barSize.x = startOutlineWidth + (unitWidth * stat);
            float widthDiff = barSize.x - oldWidth;
            if (Mathf.Abs(widthDiff) < 0.001f) return;
            Vector3 targetPosition = barFill.localPosition;
            targetPosition.x += widthDiff / 2;
            barWidth.AdvanceToVectors(targetPosition, barSize);
        }

        public void UpdateBarWithoutAnimation()
        {
            Vector2 barSize = fillRend.size;
            float oldWidth = barSize.x;
            int stat = ReadStat();
            barSize.x = startOutlineWidth + (unitWidth * stat);
            float widthDiff = barSize.x - oldWidth;
            Vector3 targetPosition = barFill.localPosition;
            targetPosition.x += widthDiff / 2;
            barWidth.SetVectorsWithoutAnimation(targetPosition, barSize);
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

        public bool IsAnimating()
        {
            return barWidth.CoroutineCount > 0;
        }
    }
}