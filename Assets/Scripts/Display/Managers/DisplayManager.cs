using Berty.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Berty.Display.Managers
{
    public class DisplayManager : ManagerSingleton<DisplayManager>
    {
        private LookupCard lookupCard;

        protected override void Awake()
        {
            InitializeSingleton();
            lookupCard = ObjectReadManager.Instance.LookupCard.GetComponent<LookupCard>();
        }

        public void ShowLookupCard(Sprite sprite)
        {
            lookupCard.ShowLookupCard(sprite);
        }

        public void HideLookupCard()
        {
            lookupCard.HideLookupCard();
        }
    }
}
