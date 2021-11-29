using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        [SerializeField] private int sceneInd = -1;
        
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneInd);
            print("Scene loaded");
            Destroy(gameObject);
        }
    }
}