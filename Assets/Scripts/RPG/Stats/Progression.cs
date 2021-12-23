using System.Linq;
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

        public float GetStat(CharacterClass characterClass, Stat stat, int level) {
            return characterClasses.First(p => p.CharacterClass == characterClass)
                .stats.First(p => p.stat == stat).levels[level-1];
        }
    }
}