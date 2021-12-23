using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes {
    public class XPDisplay : MonoBehaviour {
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