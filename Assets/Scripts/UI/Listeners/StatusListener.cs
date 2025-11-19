using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Managers;
using System;
using UnityEngine;

namespace Berty.UI.Listeners
{
    public class StatusListener : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnStatusUpdated += HandleStatusUpdated;
            EventManager.Instance.OnStatusRemoved += HandleStatusRemoved;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnStatusUpdated -= HandleStatusUpdated;
            EventManager.Instance.OnStatusRemoved -= HandleStatusRemoved;
        }

        private void HandleStatusUpdated(object sender, EventArgs args)
        {
            Status status = (Status)sender;
            switch (status.Name)
            {
                case StatusEnum.ClickToApplyEffect:
                    ButtonObjectManager.Instance.HideCornerButton();
                    break;
            }
        }

        private void HandleStatusRemoved(object sender, StatusEventArgs args)
        {
            switch (args.StatusName)
            {
                case StatusEnum.ClickToApplyEffect:
                    ButtonObjectManager.Instance.DisplayEndTurnButton();
                    break;
            }
        }
    }
}
