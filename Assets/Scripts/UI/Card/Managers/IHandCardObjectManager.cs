using Berty.BoardCards.ConfigData;
using UnityEngine;

namespace Berty.UI.Card.Managers
{
    public interface IHandCardObjectManager
    {
        void AddCardObjects();

        void RemoveCardObjects();

        Sprite GetSpriteFromHandCardObject(CharacterConfig characterConfig);

        void AddCardObjectFromConfig(CharacterConfig characterConfig);
    }
}
