
using UnityEngine;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;

        private void Update() {
            GameObject player = GameObject.FindWithTag("Player");
            if (Vector3.Distance(transform.position, player.transform.position) < chaseRange) {
                print($"{name} should chase player");
            }
        }
    }
}
