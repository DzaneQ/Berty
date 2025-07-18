using Berty.CardTransfer.Managers;
using Berty.Gameplay.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using UnityEngine;

namespace Berty.CardTransfer.Listeners
{
    public class TurnListener : MonoBehaviour
    {
        private Game game;
        private SelectionAndPaymentSystem selectionSystem;

        private void Awake()
        {
            game = CoreManager.Instance.Game;
            selectionSystem = CoreManager.Instance.SelectionAndPaymentSystem;
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
            selectionSystem.ClearSelection(); // TODO: Move to another listener and correct the code so hand card objects look unselected.
            int totalCardCount = game.GameConfig.TableCapacity;
            PileToHandManager.Instance.PullCardsTo(totalCardCount);
        }
    }
}