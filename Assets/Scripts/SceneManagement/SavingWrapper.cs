using System;
using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour {
        private const string defaultSaveFile = "save";

        private IEnumerator Start() {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(1f);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
            
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}