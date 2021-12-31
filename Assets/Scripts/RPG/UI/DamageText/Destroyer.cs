using UnityEngine;

namespace RPG.UI.DamageText {
    public class Destroyer : MonoBehaviour {
        [SerializeField] private GameObject targetToDestroy;

        public void DestroyTarget() {
            Destroy(targetToDestroy);
        }
    }
}