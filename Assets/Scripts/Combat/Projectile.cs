using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed;

        private Health aimTarget;
        private float damage;

        private void Update() {
            if (!aimTarget) {
                Destroy(gameObject);
                return;
            }

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health aimTarget, float damage) {
            this.aimTarget = aimTarget;
            this.damage = damage;
        }

        private Vector3 GetAimLocation() {
            Collider targetCollider = aimTarget.GetComponent<Collider>();
            if (targetCollider == null) {
                return aimTarget.transform.position;
            }

            return aimTarget.transform.position + Vector3.up * targetCollider.bounds.size.y / 2;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out Health health)) {
                if (aimTarget != health) return;
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}