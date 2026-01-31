using Berty.BoardCards.ConfigData;
using Berty.Characters.Init;
using Berty.Display;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Berty.UI.Card.Init
{
    public class HandCardInitialization : MonoBehaviour
    {
        [SerializeField] private GameObject cardImagePrefab;
        [SerializeField] private GameObject pileStack;
        [SerializeField] private GameObject discardStack;
        [SerializeField] private GameObject deadStack;
        [SerializeField] private GameObject playerTable;
        [SerializeField] private GameObject opponentTable;
        [SerializeField] private GameObject theRemainder;

        private int debugIndex = 1;

        private void Awake()
        {
            if (cardImagePrefab != null) cardImagePrefab = Resources.Load<GameObject>("Prefabs/CardImage");
        }

        public List<HandCardBehaviour> InitializeAllCharacterCards()
        {
            Game game = EntityLoadManager.Instance.Game;
            List<HandCardBehaviour> behaviourCollection = new();
            HideTableOpposedTo(game.CurrentAlignment);
            InitializeAllPileCards(game.CardPile.PileCards, ref behaviourCollection);
            InitializeAllDiscardedCards(game.CardPile.DiscardedCards, ref behaviourCollection);
            InitializeAllDeadCards(game.CardPile.DeadCards, ref behaviourCollection);
            InitializeAllPlayerCards(game.CardPile.PlayerCards, ref behaviourCollection);
            InitializeAllOpponentCards(game.CardPile.OpponentCards, ref behaviourCollection);
            InitializeAllFieldCards(game.Grid.GetAllCharactersOnFields(), ref behaviourCollection);
            return behaviourCollection;
        }

        private void HideTableOpposedTo(AlignmentEnum align)
        {
            switch (align)
            {
                case AlignmentEnum.Player:
                    opponentTable.SetActive(false);
                    break;
                case AlignmentEnum.Opponent:
                    playerTable.SetActive(false);
                    break;
                default:
                    throw new System.Exception("Unknown alignment to hide table");
            }
        }

        private void InitializeAllPileCards(IReadOnlyList<CharacterConfig> charactersInPile, ref List<HandCardBehaviour> behaviourCollection)
        {
            //Debug.Log("Initializing pile cards.");
            AssignCharactersToStack(pileStack, charactersInPile, ref behaviourCollection);
        }

        private void InitializeAllDiscardedCards(IReadOnlyList<CharacterConfig> charactersDiscarded, ref List<HandCardBehaviour> behaviourCollection)
        {
            //Debug.Log("Initializing discarded cards.");
            AssignCharactersToStack(discardStack, charactersDiscarded, ref behaviourCollection);
        }

        private void InitializeAllDeadCards(IReadOnlyList<CharacterConfig> charactersDead, ref List<HandCardBehaviour> behaviourCollection)
        {
            //Debug.Log("Initializing dead cards.");
            AssignCharactersToStack(deadStack, charactersDead, ref behaviourCollection);
        }

        private void InitializeAllPlayerCards(IReadOnlyList<CharacterConfig> tableCharacters, ref List<HandCardBehaviour> behaviourCollection)
        {
            //Debug.Log("Initializing player cards.");
            AssignCharactersToStack(playerTable, tableCharacters, ref behaviourCollection);
        }

        private void InitializeAllOpponentCards(IReadOnlyList<CharacterConfig> tableCharacters, ref List<HandCardBehaviour> behaviourCollection)
        {
            //Debug.Log("Initializing opponent cards.");
            AssignCharactersToStack(opponentTable, tableCharacters, ref behaviourCollection);
        }

        private void InitializeAllFieldCards(IReadOnlyList<CharacterConfig> tableCharacters, ref List<HandCardBehaviour> behaviourCollection)
        {
            //Debug.Log("Initializing field cards.");
            AssignCharactersToStack(theRemainder, tableCharacters, ref behaviourCollection);
        }

        private void AssignCharactersToStack(GameObject stack, IReadOnlyList<CharacterConfig> characters, ref List<HandCardBehaviour> behaviourCollection)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                //Debug.Log("Stack card: " + (i+1));
                //Debug.Log("Total card: " + debugIndex);
                //Debug.Log("Character name: " + characters[i].Name);
                debugIndex++;
                GameObject handCardObject = Instantiate(cardImagePrefab, stack.transform);
                HandCardBehaviour handCardBehaviour = handCardObject.GetComponent<HandCardBehaviour>();
                if (handCardBehaviour == null) handCardBehaviour = handCardObject.AddComponent<HandCardBehaviour>();
                handCardBehaviour.AssignCharacter(characters[i]);
                handCardBehaviour.name = characters[i].Name;
                behaviourCollection.Add(handCardBehaviour);
                //Debug.Log($"Assigned {characters[i].Name} to {stack.name}.");
            }
        }
    }
}