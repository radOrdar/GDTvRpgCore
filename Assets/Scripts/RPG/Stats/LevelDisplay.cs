using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats {
    public class LevelDisplay : MonoBehaviour {
        [SerializeField] private Text levelValueText;
        private BaseStats baseStats;

        private void Start() {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update() {
            levelValueText.text = $"{baseStats.GetLevel()}";
        }
    }
}