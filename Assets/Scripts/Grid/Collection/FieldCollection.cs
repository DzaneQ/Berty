using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.Grid.Collection
{
    public class FieldCollection : MonoBehaviour
    {
        private List<FieldBehaviour> fieldBehaviourCollection;

        public void InitializeCollection(List<FieldBehaviour> collection)
        {
            if (fieldBehaviourCollection != null) throw new Exception("Field collection is already initialized");
            fieldBehaviourCollection = collection;
        }

        public FieldBehaviour GetBehaviourFromEntity(BoardField boardField)
        {
            return fieldBehaviourCollection.Find((FieldBehaviour behaviour) => behaviour.BoardField == boardField);
        }
    }
}