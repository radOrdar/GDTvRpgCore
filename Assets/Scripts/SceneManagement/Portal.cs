using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        public enum DestinationIdentifier {
            A,B,C,D,E
        }
        
        [SerializeField] private int sceneInd = -1;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeDuration = 2;
        public Transform spawnPoint;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            if (sceneInd < 0) {
                Debug.LogError("Scene to load is not set.");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return StartCoroutine(fader.FadeOut(fadeDuration));
            FindObjectOfType<SavingWrapper>().Save();
            yield return SceneManager.LoadSceneAsync(sceneInd);
            FindObjectOfType<SavingWrapper>().Load();
            SetPlayerPosition();
            yield return StartCoroutine(fader.FadeIn(fadeDuration));
            
            Destroy(gameObject);
        }

        private void SetPlayerPosition() {
            Transform spawnPoint = FindObjectsOfType<Portal>().First(p => p != this && p.destination == destination).spawnPoint.transform;
            Transform player = GameObject.FindWithTag("Player").transform;
            player.GetComponent<NavMeshAgent>().Warp(spawnPoint.position);
            player.rotation =spawnPoint.rotation;
        }
        
    }
}