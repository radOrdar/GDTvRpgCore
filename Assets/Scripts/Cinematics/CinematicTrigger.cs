using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicTrigger : MonoBehaviour {
        private bool isActivated = false;
        
        private void OnTriggerEnter(Collider other) {
            if (isActivated || !other.CompareTag("Player")) return;

            isActivated = true;
            var playableDirector = GetComponent<PlayableDirector>();
            playableDirector.Play(); ;
        }
    }
}