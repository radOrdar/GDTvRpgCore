using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour {
        [SerializeField] private Text healthValueText;
        private const string defaultHealthText = "N/A";
        private Fighter playerFighter;

        private void Awake() {
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            Health targetHealth = playerFighter.Target;
            healthValueText.text = targetHealth ? $"{targetHealth.GetPercentage():0}%/{targetHealth.GetMaxHealth()}" : defaultHealthText;
        }
    }
}