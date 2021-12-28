using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats {
    public class ExperienceDisplay : MonoBehaviour {
        [SerializeField] private Text xpValueText;
        private Experience xp;

        private void Awake() {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() {
            xpValueText.text = $"{xp.ExperiencePoints:0}";
        }
    }
}