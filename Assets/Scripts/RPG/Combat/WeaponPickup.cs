using System.Collections;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour {
        [SerializeField] private float hideTime = 5f;
        [SerializeField] private WeaponSO weaponSo;
        
        private Collider collider;

        private void Awake() {
            collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Fighter>().EquipWeapon(weaponSo);
                StartCoroutine(HideRoutine());
            }
        }

        private IEnumerator HideRoutine() {
            TogglePickup(false);
            yield return new WaitForSeconds(hideTime);
            TogglePickup(true);
        }

        private void TogglePickup(bool isEnable) {
            collider.enabled = isEnable;

            foreach (Transform child in transform) {
                child.gameObject.SetActive(isEnable);
            }
        }
        
    }
}