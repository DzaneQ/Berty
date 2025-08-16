using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.UI.Card.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.Grid.Managers
{
    public class FieldCollectionManager : ManagerSingleton<FieldCollectionManager>
    {
        private List<FieldBehaviour> fieldBehaviourCollection;

        protected override void Awake()
        {
            base.Awake();
            InitializeCollection();
        }

        public void InitializeCollection()
        {
            if (fieldBehaviourCollection != null) throw new Exception("Field collection is already initialized");
            fieldBehaviourCollection = ObjectReadManager.Instance.FieldBoard.GetComponentsInChildren<FieldBehaviour>().ToList();
            if (fieldBehaviourCollection.Count != 9) throw new Exception($"Got wrong amount of fields: {fieldBehaviourCollection.Count}");
        }

        public FieldBehaviour GetBehaviourFromEntity(BoardField boardField)
        {
            return fieldBehaviourCollection.Find((FieldBehaviour behaviour) => behaviour.BoardField == boardField);
        }
    }
}