using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour {
        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update() {
            UpdateAnimator();
        }

        private void UpdateAnimator() {
            animator.SetFloat("forwardSpeed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
        }

        public void MoveTo(Vector3 point) {
            navMeshAgent.SetDestination(point);
            navMeshAgent.isStopped = false;
        }

        public void Stop() {
            navMeshAgent.isStopped = true;
        }
    }
}