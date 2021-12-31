using UnityEngine;

namespace RPG.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour {
        [SerializeField] private DamageText damageText;

        private void Start() {
            Spawn(1);
        }

        private void Spawn(float damage) {
            Instantiate(damageText, transform.position, Quaternion.identity);
        }
    }
}