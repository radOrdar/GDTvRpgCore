using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
    }
}