using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {

        [SerializeField] private ProgressionCharacter[] characterClasses;

        [System.Serializable]
        class ProgressionCharacter {
            public CharacterClass CharacterClass;
            public float[] healthProgression;
        }
    }
}