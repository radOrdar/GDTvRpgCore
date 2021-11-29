using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        [SerializeField] private int sceneInd = -1;
        
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                SceneManager.LoadScene(sceneInd);
            }
        }
    }
}