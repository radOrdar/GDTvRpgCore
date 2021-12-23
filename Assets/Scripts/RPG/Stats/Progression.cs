using System.Linq;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [System.Serializable]
        public class ProgressionCharacter {
            public CharacterClass CharacterClass;
            public float[] healthProgression;
        }

        [SerializeField] private ProgressionCharacter[] characterClasses;

        public float GetHealth(CharacterClass characterClass, int level) {
            return characterClasses.First(p => p.CharacterClass == characterClass).healthProgression[level - 1];
        }
        
        
    }
}