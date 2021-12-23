using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] private Progression progression;

        public float GetHealth() {
            return progression.GetHealth(characterClass, startingLevel);
        }
    }
}