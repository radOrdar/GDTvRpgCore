using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed;

        private Health aimTarget;

        private void Update() {
            if (!aimTarget) return;
            
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target) {
            aimTarget = target;
        }

        private Vector3 GetAimLocation() {
            Collider targetCollider = aimTarget.GetComponent<Collider>();
            if (targetCollider == null) {
                return aimTarget.transform.position;
            }

            return aimTarget.transform.position + Vector3.up * targetCollider.bounds.size.y / 2;
        }
    }
}