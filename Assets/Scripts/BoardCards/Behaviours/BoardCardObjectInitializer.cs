using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Display.View;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardObjectInitializer : BoardCardBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            BoardCardCollectionManager.Instance.AddCardToCollection(this);
        }

        private void Start()
        {
            DisableBackupCard();
            AdjustInitRotation();
            ParentField.UpdateField();
        }

        private void DisableBackupCard()
        {
            GameObject backupCard = transform.parent.GetChild(0).gameObject;
            if (backupCard == gameObject) return;
            backupCard.SetActive(false);
        }
        
        private void AdjustInitRotation()
        {
            if (transform.parent.childCount > 1) return; // Keep the backup card's rotation.
            int rightAngle = (180 - Mathf.RoundToInt(Camera.main.GetComponent<RotateCamera>().RightAngleValue())) % 360;
            Navigation.RotateObjectWithoutAnimation(rightAngle);
            BoardCard.AdvanceCardSetAngleBy(rightAngle);
        }
    }
}