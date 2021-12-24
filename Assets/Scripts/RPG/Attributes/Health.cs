using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] private float levelupRegenPercentage = 70;
        
        private float healthPoints = -1;
        private BaseStats baseStats;

        public bool IsDead => healthPoints == 0;

        private void Awake() {
            baseStats = GetComponent<BaseStats>();
        }

        private void Start() {
            if (healthPoints < 0) {
                healthPoints = baseStats.GetStat(Stat.Health);
            }
        }

        private void OnEnable() {
            baseStats.OnLevelUp += HandleLevelUp;
        }

        private void OnDisable() {
            baseStats.OnLevelUp -= HandleLevelUp;
        }

        private void HandleLevelUp() {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * levelupRegenPercentage / 100;
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage) {
            if (healthPoints == 0) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0) {
                Experience experience = instigator.GetComponent<Experience>();
                if (experience) experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
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