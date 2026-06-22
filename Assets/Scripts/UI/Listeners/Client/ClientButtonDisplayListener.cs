using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Network.Managers;
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
            if (NetworkTurnManager.Instance.IsItMyTurn()) ButtonObjectManager.Instance.DisplayEndTurnButton();
            else ButtonObjectManager.Instance.HideCornerButton();
        }
    }
}