using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        private GameObject player;
        private PlayableDirector playableDirector;
        private ActionScheduler playerActionScheduler;
        private PlayerController playerController;
        
        private void Awake() {
            player = GameObject.FindWithTag("Player");
            playableDirector = GetComponent<PlayableDirector>();
            playerActionScheduler = player.GetComponent<ActionScheduler>();
            playerController = player.GetComponent<PlayerController>();
        }

        private void OnEnable() {
            playableDirector.played += DisableControls;
            playableDirector.stopped += EnableControls;
        }

        private void OnDisable() {
            playableDirector.played -= DisableControls;
            playableDirector.stopped -= EnableControls;
        }

        void DisableControls(PlayableDirector playableDirector) {
            playerActionScheduler = player.GetComponent<ActionScheduler>();
            playerActionScheduler.CancelCurrentAction();
            playerController.enabled = false;
        }

        void EnableControls(PlayableDirector playableDirector) {
            playerController.enabled = true;
        }
    }
}