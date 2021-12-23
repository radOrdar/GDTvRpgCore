using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed;
        [SerializeField] private bool isHoming;
        [SerializeField] private ParticleSystem impactFX;
        [SerializeField] private GameObject[] destroyOnHit = {};
        [SerializeField] private float lifeAfterImpact = .2f;
        
        private Health aimTarget;
        private float damage;

        private void Update() {
            if (isHoming && aimTarget && !aimTarget.IsDead) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health aimTarget, float damage) {
            this.aimTarget = aimTarget;
            transform.LookAt(GetAimLocation());
            this.damage = damage;
            Destroy(gameObject, 10f);
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
                if (!aimTarget || aimTarget != health || aimTarget.IsDead) return;
                health.TakeDamage(damage);
                speed = 0;
                if(impactFX) Instantiate(impactFX, GetAimLocation(), transform.rotation);
                foreach (var go in destroyOnHit) {
                    Destroy(go);
                }
                Destroy(gameObject, lifeAfterImpact);
            }
        }
    }
}