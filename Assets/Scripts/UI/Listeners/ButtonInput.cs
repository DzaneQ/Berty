using Berty.Audio;
using Berty.Gameplay;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.UI.Listeners
{
    public class ButtonInput : MonoBehaviour, IPointerUpHandler
    {
        private Turn turn;
        private SoundSystem soundSystem;

        void Start()
        {
            turn = FindObjectOfType<Turn>();
            soundSystem = FindObjectOfType<SoundSystem>();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(0) && eventData.pointerCurrentRaycast.gameObject == transform.GetChild(0).gameObject) HandleTheButtonClick();
        }

        private void HandleTheButtonClick()
        {
            Debug.Log("Left button click is being handled.");
            soundSystem.ButtonClickSound();
            switch (turn.TheButtonText)
            {
                case "Koniec tury":
                    TurnManager.Instance.EndTurn();
                    break;
                case "Cofnij":
                    turn.FG.CancelCard();
                    break;
                default:
                    throw new Exception("Unknown ending button!");
            }
        }
    }
}
