using System;
using System.Linq;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()).OrderBy(hit => hit.distance).ToArray();
            foreach (RaycastHit hit in hits) {
                foreach (IRaycastable raycastable in hit.transform.GetComponents<IRaycastable>()) {
                    if (raycastable.HandleRaycast(this)) {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InteractWithMovement() {
            if (!RaycastNavMesh(out Vector3 target)) return false;
            if (!mover.CanMove(target)) return false;
            
            SetCursor(CursorType.Movement);
            
            if (Input.GetMouseButton(0)) {
                mover.StartMoveAction(target);
            }

            return true;
        }

        private bool RaycastNavMesh(out Vector3 target) {
            Ray ray = GetMouseRay();
            target = Vector3.zero;
            if (!Physics.Raycast(ray, out RaycastHit hit) ||
                !NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, .5f, NavMesh.AllAreas)) return false;

            target = navMeshHit.position;
            return true;
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