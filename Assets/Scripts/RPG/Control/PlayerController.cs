using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        private Mover mover;
        private Camera mainCamera;
        private Fighter fighter;
        private Health health;

        private void Awake() {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Start() {
            mainCamera = Camera.main;
        }

        private void Update() {
            if (health.IsDead) {
                enabled = false;
                return;
            }
            if (InteractWithCombat()) {
                return;
            }

            if (!InteractWithMovement()) {
                // print("Nothing to do.");
            }
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(ScreenPointToRay());
            foreach (RaycastHit hit in hits) {
                CombatTarget combatTarget = hit.transform.GetComponent<CombatTarget>();
                if (combatTarget == null) continue;
                if (fighter.CanAttack(combatTarget.gameObject)) {
                    if (Input.GetMouseButton(0)) {
                        fighter.Attack(combatTarget.gameObject);
                    }

                    return true;
                }
            }

            return false;
        }

        private bool InteractWithMovement() {
            Ray ray = ScreenPointToRay();
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (Input.GetMouseButton(0)) {
                   mover.StartMoveAction(hit.point);
                }

                return true;
            }
            return false;
        }

        private Ray ScreenPointToRay() {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}