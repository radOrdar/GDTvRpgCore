using UnityEngine;

namespace RPG.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour {
        [SerializeField] private DamageText damageText;

        public void Spawn(float damage) {
            DamageText damageTextInstance = Instantiate(damageText, transform);
            damageTextInstance.Init($"{damage:0}");
        }
    }
}