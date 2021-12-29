using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {
        [SerializeField] private float hideTime = 5f;
        [SerializeField] private WeaponSO weaponSo;

        private Collider myCollider;

        private void Awake() {
            myCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter) {
            fighter.EquipWeapon(weaponSo);
            StartCoroutine(HideRoutine());
        }

        private IEnumerator HideRoutine() {
            TogglePickup(false);
            yield return new WaitForSeconds(hideTime);
            TogglePickup(true);
        }

        private void TogglePickup(bool isEnable) {
            myCollider.enabled = isEnable;

            foreach (Transform child in transform) {
                child.gameObject.SetActive(isEnable);
            }
        }

        public bool HandleRaycast(PlayerController callerController) {
            if (Input.GetMouseButtonDown(0)) {
                if (Vector3.Distance(transform.position, callerController.transform.position) < 5) {
                    Pickup(callerController.GetComponent<Fighter>());
                }
            }

            return true;
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }
    }
}