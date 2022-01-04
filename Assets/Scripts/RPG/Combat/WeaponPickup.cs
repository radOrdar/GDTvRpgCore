using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {
        [SerializeField] private float healthToRestore = 0;
        [SerializeField] private float hideTime = 5f;
        [FormerlySerializedAs("weaponSo")]
        [SerializeField] private WeaponConfigSO weaponConfigSo;

        private Collider myCollider;

        private void Awake() {
            myCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
              Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject) {
            if (weaponConfigSo) {
                subject.GetComponent<Fighter>().EquipWeapon(weaponConfigSo);
            }

            if (healthToRestore > 0) {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
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
                    Pickup(callerController.gameObject);
                }
            }

            return true;
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }
    }
}