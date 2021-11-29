using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        [SerializeField] private int sceneInd = -1;
        public Transform spawnPoint;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneInd);
            
            SetPlayerPosition();
            
            Destroy(gameObject);
        }

        private void SetPlayerPosition() {
            Transform spawnPoint = FindObjectsOfType<Portal>().First(p => p != this).spawnPoint.transform;
            Transform player = GameObject.FindWithTag("Player").transform;
            player.GetComponent<NavMeshAgent>().Warp(spawnPoint.position);
            player.rotation =spawnPoint.rotation;
        }
        
    }
}