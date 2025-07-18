using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class ObjectReadManager : ManagerSingleton<ObjectReadManager>
    {

        [SerializeField] private GameObject handCardObjectCollection;
        [SerializeField] private GameObject playerTable;
        [SerializeField] private GameObject opponentTable;
        [SerializeField] private GameObject lookupCard;
        [SerializeField] private GameObject fieldBoard;

        public GameObject HandCardObjectCollection => handCardObjectCollection;
        public GameObject PlayerTable => playerTable;
        public GameObject OpponentTable => opponentTable;
        public GameObject LookupCard => lookupCard;
        public GameObject FieldBoard => fieldBoard;
    }
}