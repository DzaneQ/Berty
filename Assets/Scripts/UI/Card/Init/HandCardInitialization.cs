using Berty.BoardCards.ConfigData;
using Berty.Characters.Init;
using Berty.Display;
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

        private void Awake()
        {
            if (cardImagePrefab != null) cardImagePrefab = Resources.Load<GameObject>("Prefabs/CardImage");
        }

        public List<HandCardBehaviour> InitializeAllCharacterCards()
        {
            Game game = EntityLoadManager.Instance.Game;
            List<HandCardBehaviour> behaviourCollection = new();
            InitializeAllPileCards(game.CardPile.PileCards, ref behaviourCollection);
            InitializeAllDiscardedCards(game.CardPile.DiscardedCards, ref behaviourCollection);
            InitializeAllDeadCards(game.CardPile.DeadCards, ref behaviourCollection);
            InitializeAllPlayerCards(game.CardPile.PlayerCards, ref behaviourCollection);
            InitializeAllOpponentCards(game.CardPile.OpponentCards, ref behaviourCollection);
            return behaviourCollection;
        }

        private void InitializeAllPileCards(IReadOnlyList<CharacterConfig> charactersInPile, ref List<HandCardBehaviour> behaviourCollection)
        {
            AssignCharactersToStack(pileStack, charactersInPile, ref behaviourCollection);
        }

        private void InitializeAllDiscardedCards(IReadOnlyList<CharacterConfig> charactersDiscarded, ref List<HandCardBehaviour> behaviourCollection)
        {
            AssignCharactersToStack(discardStack, charactersDiscarded, ref behaviourCollection);
        }

        private void InitializeAllDeadCards(IReadOnlyList<CharacterConfig> charactersDead, ref List<HandCardBehaviour> behaviourCollection)
        {
            AssignCharactersToStack(deadStack, charactersDead, ref behaviourCollection);
        }

        private void InitializeAllPlayerCards(IReadOnlyList<CharacterConfig> tableCharacters, ref List<HandCardBehaviour> behaviourCollection)
        {
            AssignCharactersToStack(playerTable, tableCharacters, ref behaviourCollection);
        }

        private void InitializeAllOpponentCards(IReadOnlyList<CharacterConfig> tableCharacters, ref List<HandCardBehaviour> behaviourCollection)
        {
            AssignCharactersToStack(opponentTable, tableCharacters, ref behaviourCollection);
        }

        private void AssignCharactersToStack(GameObject stack, IReadOnlyList<CharacterConfig> characters, ref List<HandCardBehaviour> behaviourCollection)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                GameObject handCardObject = Instantiate(cardImagePrefab, stack.transform);
                HandCardBehaviour handCardBehaviour = handCardObject.GetComponent<HandCardBehaviour>() ?? handCardObject.AddComponent<HandCardBehaviour>();
                handCardBehaviour.AssignCharacter(characters[i]);
                handCardBehaviour.name = characters[i].Name;
                behaviourCollection.Add(handCardBehaviour);
            }
        }
    }
}