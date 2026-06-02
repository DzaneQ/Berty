using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.Gameplay.Managers.Shared;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using UnityEngine;

namespace Berty.UI.Listeners.Client
{
    public class ClientButtonDisplayListener : MonoBehaviour
    {
        private Game game;

        private void Awake()
        {
            game = EntityLoadManager.Instance.Game;
            EventManager.Instance.OnNewTurn += HandleNewTurn; // should listen even if disabled
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewTurn -= HandleNewTurn;
        }

        private void HandleNewTurn()
        {
            Debug.Log($"Checking client button when new turn for {SharedTurnManager.Instance.CurrentAlignment}");
            if (PlayerReadManager.Instance.IsItMyTurn()) ButtonObjectManager.Instance.DisplayEndTurnButton();
            else ButtonObjectManager.Instance.HideCornerButton();
        }
    }
}