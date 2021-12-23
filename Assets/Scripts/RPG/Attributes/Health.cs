using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] private float health = 100;
        
        public bool IsDead => health == 0;

        private void Start() {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float damage) {
            if (health == 0) return;
            health = Mathf.Max(health - damage, 0);
            if (health == 0) {
                Die();
            }
        }

        private void Die() {
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState() {
            return health;
        }

        public void RestoreState(object state) {
            health = (float)state;
            if (health == 0) {
                Die();
            }
        }
    }
}