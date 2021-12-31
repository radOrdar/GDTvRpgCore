using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] private float levelupRegenPercentage = 70;
        [SerializeField] private UnityEvent<float> takeDamage;

        private LazyValue<float> healthPoints;
        public bool IsDead => healthPoints.value <= 0;

        private BaseStats baseStats;
        
        private void Awake() {
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(() => baseStats.GetStat(Stat.Health));
        }

        private void OnEnable() => baseStats.OnLevelUp += HandleLevelUp;

        private void OnDisable() => baseStats.OnLevelUp -= HandleLevelUp;

        private void Start() {
            healthPoints.ForceInit();
        }
        
        private void HandleLevelUp() {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * levelupRegenPercentage / 100;
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage) {
            print(gameObject.name + $" took damage: {damage}");
            
            if (healthPoints.value == 0) return;
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamage?.Invoke(damage);
            if (healthPoints.value == 0) {
                Experience experience = instigator.GetComponent<Experience>();
                if (experience) experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
                Die();
            }
        }

        private void Die() {
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetPercentage() => 100 * healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);

        public float GetMaxHealth() => baseStats.GetStat(Stat.Health);

        public object CaptureState() => healthPoints.value;

        public void RestoreState(object state) {
            healthPoints.value = (float)state;
            if (healthPoints.value == 0) {
                Die();
            }
        }
    }
}