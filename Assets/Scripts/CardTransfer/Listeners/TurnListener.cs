using Berty.CardTransfer.Managers;
using Berty.Gameplay.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
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
            CardManager.Instance.SelectionSystem.ClearSelection();
            int totalCardCount = game.GameConfig.TableCapacity;
            CardManager.Instance.PullCardsTo(totalCardCount);
        }
    }
}