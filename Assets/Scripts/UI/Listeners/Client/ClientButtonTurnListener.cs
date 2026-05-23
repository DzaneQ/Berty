using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using UnityEngine;

namespace Berty.UI.Listeners.Client
{
    public class ClientButtonTurnListener : MonoBehaviour
    {
        private Game game;

        private void Awake()
        {
            game = EntityLoadManager.Instance.Game;
        }

        private void OnEnable()
        {
            EventManager.Instance.OnNewTurn += HandleNewTurn;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewTurn -= HandleNewTurn;
        }

        private void HandleNewTurn()
        {
            Debug.Log($"New turn for {EntityLoadManager.Instance.Game.CurrentAlignment}");
            if (!PlayerReadManager.Instance.IsMyAlignment(game.CurrentAlignment))
            {
                ButtonObjectManager.Instance.HideCornerButton();
            }
            else
            {
                ButtonObjectManager.Instance.DisplayEndTurnButton();
            }
        }
    }
}