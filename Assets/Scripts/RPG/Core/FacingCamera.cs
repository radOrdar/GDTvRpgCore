using UnityEngine;

namespace RPG.Core {
    public class FacingCamera : MonoBehaviour {

        private Camera mainCamera;

        private void Start() {
            mainCamera = Camera.main;
        }

        private void LateUpdate() {
            transform.LookAt(mainCamera.transform.position);
        }
    }
}