using Berty.UI.Card.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardRigidbody : MonoBehaviour
    {
        private Rigidbody cardRB;

        private void Awake()
        {
            InitializeRigidbody();
        }

        private void OnCollisionEnter(Collision collision)
        {
            ApplyPhysics(false);
        }

        private void InitializeRigidbody()
        {
            cardRB = GetComponent<Rigidbody>();
            cardRB.detectCollisions = true;
            ApplyPhysics(true);
        }

        private void ApplyPhysics(bool isApplied = true)
        {
            cardRB.isKinematic = !isApplied;
        }
    }
}