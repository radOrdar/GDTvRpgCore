using System;
using System.Linq;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        enum CursorType {
            None = 0,
            Movement = 1,
            Combat = 2,
            UI = 3
        }

        [Serializable]
        struct CursorMapping {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings;

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
            if (InteractWithUI()) return;
            if (health.IsDead) {
                enabled = false;
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
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

                    SetCursor(CursorType.Combat);

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

                SetCursor(CursorType.Movement);

                return true;
            }

            return false;
        }

        private bool InteractWithUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        private void SetCursor(CursorType cursorType) {
            CursorMapping mapping = cursorMappings.First(p => p.type == cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private Ray ScreenPointToRay() {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}