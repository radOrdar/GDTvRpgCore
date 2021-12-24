using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {
        private float healthPoints = -1;

        public bool IsDead => healthPoints == 0;

        private void Start() {
            if (healthPoints < 0) {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public void TakeDamage(GameObject instigator, float damage) {
            if (healthPoints == 0) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0) {
                Experience experience = instigator.GetComponent<Experience>();
                if (experience) experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
                Die();
            }
        }

        private void Die() {
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetPercentage() {
            return 100 * healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState() {
            return healthPoints;
        }

        public void RestoreState(object state) {
            healthPoints = (float)state;
            if (healthPoints == 0) {
                Die();
            }
        }
    }
}