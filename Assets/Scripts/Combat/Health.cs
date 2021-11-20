using UnityEngine;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] private float health = 100;

        public void TakeDamage(float damage) {
            if (health == 0) return;
            health = Mathf.Max(health - damage, 0);
            if (health == 0) {
                GetComponent<Animator>().SetTrigger("death");
            }
        }
    }
}