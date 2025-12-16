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
        [SerializeField] private GameObject cornerButton;
        [SerializeField] private GameObject deadCardsScreen;
        [SerializeField] private GameObject escapeCanvas;
        [SerializeField] private GameObject backupCard;

        public GameObject HandCardObjectCollection => handCardObjectCollection;
        public GameObject PlayerTable => playerTable;
        public GameObject OpponentTable => opponentTable;
        public GameObject LookupCard => lookupCard;
        public GameObject FieldBoard => fieldBoard;
        public GameObject CornerButton => cornerButton;
        public GameObject DeadCardsScreen => deadCardsScreen;
        public GameObject EscapeCanvas => escapeCanvas;
        public GameObject BackupCard => backupCard;
    }
}