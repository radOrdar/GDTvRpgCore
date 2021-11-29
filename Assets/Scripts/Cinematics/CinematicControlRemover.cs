using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        private void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControls;
            GetComponent<PlayableDirector>().stopped += EnableControls;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControls;
            GetComponent<PlayableDirector>().stopped -= EnableControls;
        }

        void DisableControls(PlayableDirector playableDirector) {
            print("Disable controls");
        }

        void EnableControls(PlayableDirector playableDirector) {
            print("Enable controls");
        }
    }
}