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

        public void TakeDamage(GameObject instigator, float damage) {
            if (health == 0) return;
            health = Mathf.Max(health - damage, 0);
            if (health == 0) {
                Experience experience = instigator.GetComponent<Experience>();
                if (experience) experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
                Die();
            }
        }

        private void Die() {
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetPercentage() {
            return 100 * health / GetComponent<BaseStats>().GetHealth();
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