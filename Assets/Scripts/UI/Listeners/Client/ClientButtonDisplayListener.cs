using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.Network.Managers.Shared;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Newtonsoft.Json.Bson;
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

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewTurn -= HandleNewTurn;
        }

        private void HandleNewTurn()
        {
            if (SharedTurnManager.Instance.IsItMyTurn()) ButtonObjectManager.Instance.DisplayEndTurnButton();
            else ButtonObjectManager.Instance.HideCornerButton();
        }
    }
}