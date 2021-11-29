using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        private GameObject player;
        
        private void Start() {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControls;
            GetComponent<PlayableDirector>().stopped += EnableControls;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControls;
            GetComponent<PlayableDirector>().stopped -= EnableControls;
        }

        void DisableControls(PlayableDirector playableDirector) {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControls(PlayableDirector playableDirector) {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}