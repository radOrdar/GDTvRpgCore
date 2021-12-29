using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat {
    
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable {
        
        public bool HandleRaycast(PlayerController callerController) {
            Fighter fighter = callerController.GetComponent<Fighter>();
            if (fighter.CanAttack(gameObject)) {
                if (Input.GetMouseButton(0)) {
                    fighter.Attack(gameObject);
                }
                return true;
            }

            return false;
        }
    }
}