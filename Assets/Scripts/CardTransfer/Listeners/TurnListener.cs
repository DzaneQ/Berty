using Berty.CardTransfer.Managers;
using Berty.Gameplay.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using UnityEngine;

namespace Berty.CardTransfer.Listeners
{
    public class TurnListener : MonoBehaviour
    {
        private Game game;

        private void Awake()
        {
            game = CoreManager.Instance.Game;
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
            HandCardSelectManager.Instance.SelectionSystem.ClearSelection();
            int totalCardCount = game.GameConfig.TableCapacity;
            PileToHandManager.Instance.PullCardsTo(totalCardCount);
        }
    }
}