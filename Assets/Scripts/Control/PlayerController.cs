using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        private Mover mover;
        private Camera mainCamera;
        private Fighter fighter;

        private void Start() {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            mainCamera = Camera.main;
        }

        private void Update() {
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
                if (hit.collider.TryGetComponent(out CombatTarget combatTarget)) {
                    if (Input.GetMouseButtonDown(0)) {
                        fighter.Attack(combatTarget);
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