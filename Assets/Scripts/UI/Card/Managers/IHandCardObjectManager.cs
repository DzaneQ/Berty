using Berty.BoardCards.ConfigData;
using UnityEngine;

namespace Berty.UI.Card.Managers
{
    public interface IHandCardObjectManager
    {
        public void AddCardObjects();

        public void RemoveCardObjects();

        public Sprite GetSpriteFromHandCardObject(CharacterConfig characterConfig);

        public void AddCardObjectFromConfig(CharacterConfig characterConfig);
    }
}
