using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [System.Serializable]
        public class ProgressionCharacter {
            public CharacterClass CharacterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        public class ProgressionStat {
            public Stat stat;
            public float[] levels;
        }

        [SerializeField] private ProgressionCharacter[] characterClasses;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;
        
        public float GetStat(CharacterClass characterClass, Stat stat, int level) {
            BuildLookup();
            return lookupTable[characterClass][stat][level - 1];
        }

        private void BuildLookup() {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacter progressionCharacter in characterClasses) {
                Dictionary<Stat, float[]> statToLevels = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionCharacter.stats) {
                    statToLevels[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionCharacter.CharacterClass] = statToLevels;
            }
        }
    }
}