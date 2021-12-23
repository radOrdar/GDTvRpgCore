using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicTrigger : MonoBehaviour, ISaveable {
        private bool isActivated = false;
        
        private void OnTriggerEnter(Collider other) {
            if (isActivated || !other.CompareTag("Player")) return;

            isActivated = true;
            var playableDirector = GetComponent<PlayableDirector>();
            playableDirector.Play();
        }

        public object CaptureState() {
            return isActivated;
        }

        public void RestoreState(object state) {
            isActivated = (bool)state;
        }
    }
}