using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] private float levelupRegenPercentage = 70;


        public float HealthPoints { get; private set; } = -1;
        public bool IsDead => HealthPoints == 0;

        private BaseStats baseStats;
        
        private void Awake() => baseStats = GetComponent<BaseStats>();

        private void OnEnable() => baseStats.OnLevelUp += HandleLevelUp;

        private void OnDisable() => baseStats.OnLevelUp -= HandleLevelUp;
        
        private void Start() {
            if (HealthPoints < 0) {
                HealthPoints = baseStats.GetStat(Stat.Health);
            }
        }
        
        private void HandleLevelUp() {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * levelupRegenPercentage / 100;
            HealthPoints = Mathf.Max(HealthPoints, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage) {
            print(gameObject.name + $" took damage: {damage}");
            
            if (HealthPoints == 0) return;
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);
            if (HealthPoints == 0) {
                Experience experience = instigator.GetComponent<Experience>();
                if (experience) experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
                Die();
            }
        }

        private void Die() {
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetPercentage() => 100 * HealthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);

        public float GetMaxHealth() => baseStats.GetStat(Stat.Health);

        public object CaptureState() => HealthPoints;

        public void RestoreState(object state) {
            HealthPoints = (float)state;
            if (HealthPoints == 0) {
                Die();
            }
        }
    }
}