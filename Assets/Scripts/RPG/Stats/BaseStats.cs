using System;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private ParticleSystem levelupFx;

        public event Action OnLevelUp;
        
        private int currentLevel = -1;

        private void Start() {
            if (characterClass == CharacterClass.Player) {
                print("BaseStats");
            }

            currentLevel = CalculateLevel();
            if (TryGetComponent(out Experience experience)) {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        public float GetStat(Stat stat) {
            return progression.GetStat(characterClass, stat, GetLevel()) + GetAdditiveModifier(stat);
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

        private float GetAdditiveModifier(Stat stat) {
            float result = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>()) {
                foreach (float value in modifierProvider.GetAdditiveModifier(stat)) {
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