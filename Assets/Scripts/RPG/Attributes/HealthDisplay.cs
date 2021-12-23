using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes {
    public class HealthDisplay : MonoBehaviour {
        [SerializeField] private Text healthValueText;
        private Health health;

        private void Start() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            healthValueText.text =$"{health.GetPercentage():0}%";
        }
    }
}