using System;
using UnityEngine;

namespace RPG.Core {
    public class PersistentObjectSpawner : MonoBehaviour {
        [SerializeField] private GameObject persistentObjectPrefab;

        private static bool hasSpawned = false;

        private void Awake() {
            if (hasSpawned) return;

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects() {
            GameObject instance = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(instance);
        }
    }
}