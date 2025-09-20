using Berty.Audio;
using Berty.Audio.Managers;
using Berty.Enums;
using Berty.Gameplay;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
