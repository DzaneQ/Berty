using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardRigidbody : MonoBehaviour
    {
        private Rigidbody cardRB;
        private Vector3 defaultPosition;

        private void Awake()
        {
            defaultPosition = transform.position;
            InitializeRigidbody();
        }

        private void OnEnable()
        {
            if (Vector3.Distance(defaultPosition, transform.position) < 0.0001f) ApplyPhysics(true);
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