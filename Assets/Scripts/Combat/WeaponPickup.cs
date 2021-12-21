using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour {
        [SerializeField] private WeaponSO weaponSo;
    
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Fighter>().EquipWeapon(weaponSo);
                Destroy(gameObject);
            }
        }
    }
}