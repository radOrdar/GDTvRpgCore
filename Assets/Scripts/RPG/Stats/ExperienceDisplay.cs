using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats {
    public class ExperienceDisplay : MonoBehaviour {
        [SerializeField] private Text XPvalueText;
        private Experience xp;

        private void Start() {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() {
            XPvalueText.text = $"{xp.ExperiencePoints:0}";
        }
    }
}