using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes {
    public class HealthDisplay : MonoBehaviour {
        [SerializeField] private Text healthValueText;
        
        private Health health;

        private void Awake() {
            var player = GameObject.FindWithTag("Player");
            health = player.GetComponent<Health>();
        }

        private void Update() {
            healthValueText.text =$"{health.GetPercentage():0}%/{health.GetMaxHealth()}";
        }
    }
}