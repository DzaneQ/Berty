using Berty.Gameplay.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using UnityEngine;

namespace Berty.UI.Card.Listeners
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
            HandCardSelectManager.Instance.ClearSelection();
            int totalCardCount = game.GameConfig.TableCapacity;
            PileToHandManager.Instance.PullCardsTo(totalCardCount);
        }
    }
}