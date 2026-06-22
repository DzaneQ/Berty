using Berty.UI.Card.Managers;
using Berty.Display.Managers;
using UnityEngine;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Enums;

namespace Berty.UI.Card.Listeners
{
    public class TableDisplayListener : MonoBehaviour
    {
        private Game game;
        [SerializeField] private AlignmentEnum tableAlignment;

        private void Awake()
        {
            game = EntityLoadManager.Instance.Game;
            EventManager.Instance.OnNewTurn += HandleNewTurn;
        }

        private void Start()
        {
            DisplayCurrentAlignmentTable();
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewTurn -= HandleNewTurn;
        }

        private void HandleNewTurn()
        {
            DisplayCurrentAlignmentTable();
        }

        private void DisplayCurrentAlignmentTable()
        {
            gameObject.SetActive(game.CurrentAlignment == tableAlignment);
        }
    }
}
