using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour {
        [SerializeField] private float health = 100;

        public bool IsDead => health == 0;
        public void TakeDamage(float damage) {
            if (health == 0) return;
            health = Mathf.Max(health - damage, 0);
            if (health == 0) {
                GetComponent<Animator>().SetTrigger("death");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }
    }
}