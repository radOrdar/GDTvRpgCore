using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText {
    public class DamageText : MonoBehaviour {
        [SerializeField] private Text damageText;

        public void Init(string text) {
            damageText.text = text;
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}