using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.UI.Card.Collection
{
    public class HandCardCollection : MonoBehaviour
    {
        private List<HandCardBehaviour> handCardBehaviourCollection;

        public void InitializeCollection(List<HandCardBehaviour> collection)
        {
            if (handCardBehaviourCollection != null) throw new Exception("Hand card collection is already initialized");
            handCardBehaviourCollection = collection;
        }

        // NOTE: Characters are compared by names so they are not supposed to have identical names otherwise wrong comparisons can happen.
        public List<HandCardBehaviour> GetBehavioursFromCharacterConfigs(IReadOnlyList<CharacterConfig> characterConfigs)
        {
            List<string> configNames = characterConfigs.Select(x => x.Name).ToList();
            return handCardBehaviourCollection.FindAll((HandCardBehaviour behaviour) => configNames.Contains(behaviour.Character.Name));
        }

        public HandCardBehaviour GetBehaviourFromCharacterConfig(CharacterConfig characterConfig)
        {
            return handCardBehaviourCollection.Find((HandCardBehaviour behaviour) => behaviour.Character.Name == characterConfig.Name);
        }

        public List<Transform> GetTransformListFromCharacterConfigs(IReadOnlyList<CharacterConfig> characterConfigs)
        {
            List<string> configNames = characterConfigs.Select(x => x.Name).ToList();
            return handCardBehaviourCollection.FindAll((HandCardBehaviour behaviour) => configNames.Contains(behaviour.Character.Name)).ConvertAll(x => x.transform);
        }
    }
}