using System;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private ParticleSystem levelupFx;
        [SerializeField] private bool shouldUseModifier = false;

        public event Action OnLevelUp;

        private int currentLevel = -1;
        private Experience experience;

        private void Awake() {
            experience = GetComponent<Experience>();
        }

        private void OnEnable() {
            if (experience != null) {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null) {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void Start() {
            currentLevel = CalculateLevel();
           
        }

        public int GetLevel() {
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        private void UpdateLevel() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
                Instantiate(levelupFx, transform);
                OnLevelUp?.Invoke();
            }
        }

        public float GetStat(Stat stat) {
            float baseStat = progression.GetStat(characterClass, stat, GetLevel());
            return (baseStat + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetPercentageModifier(Stat stat) {
            if (!shouldUseModifier) return 0;
            
            float result = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>()) {
                foreach (float value in modifierProvider.GetPercentageModifiers(stat)) {
                    result += value;
                }
            }

            return result;
        }

        private float GetAdditiveModifier(Stat stat) {
            if (!shouldUseModifier) return 0;
            
            float result = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>()) {
                foreach (float value in modifierProvider.GetAdditiveModifiers(stat)) {
                    result += value;
                }
            }

            return result;
        }

        private int CalculateLevel() {
            if (!TryGetComponent(out Experience experience)) {
                return startingLevel;
            }

            float currentXp = experience.ExperiencePoints;
            int maxLevel = progression.GetMaxLevelForStat(characterClass, Stat.ExperienceToLevelUp);

            for (int level = 1; level < maxLevel; level++) {
                float xpToLevelUp = progression.GetStat(characterClass, Stat.ExperienceToLevelUp, level);
                if (xpToLevelUp > currentXp) {
                    return level;
                }
            }

            return maxLevel;
        }
    }
}