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

        public List<HandCardBehaviour> GetBehavioursFromCharacterConfigs(IReadOnlyList<CharacterConfig> characterConfigs)
        {
            return handCardBehaviourCollection.FindAll((HandCardBehaviour behaviour) => characterConfigs.Contains(behaviour.Character));
        }

        public HandCardBehaviour GetBehaviourFromCharacterConfig(CharacterConfig characterConfig)
        {
            return handCardBehaviourCollection.Find((HandCardBehaviour behaviour) => behaviour.Character == characterConfig);
        }

        public List<Transform> GetTransformListFromCharacterConfigs(IReadOnlyList<CharacterConfig> characterConfigs)
        {
            return handCardBehaviourCollection.FindAll((HandCardBehaviour behaviour) => characterConfigs.Contains(behaviour.Character)).ConvertAll(x => x.transform);
        }
    }
}