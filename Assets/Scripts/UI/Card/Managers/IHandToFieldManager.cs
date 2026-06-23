using Berty.BoardCards.ConfigData;
using Berty.Grid.Field.Behaviour;
using UnityEngine;

namespace Berty.UI.Card.Managers
{
    public interface IHandToFieldManager
    {
        CharacterConfig RemoveSelectedCardFromHand();

        GameObject ActivateCardOnField(FieldBehaviour field, CharacterConfig cardConfig);
    }
}
