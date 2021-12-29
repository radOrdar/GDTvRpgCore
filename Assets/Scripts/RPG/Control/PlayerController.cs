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

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits) {
                foreach (IRaycastable raycastable in hit.transform.GetComponents<IRaycastable>()) {
                    if (raycastable.HandleRaycast(this)) {
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InteractWithMovement() {
            Ray ray = GetMouseRay();
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

        private Ray GetMouseRay() {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}