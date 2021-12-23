﻿using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {
        private CanvasGroup canvasGroup;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time) {
            canvasGroup.alpha = 0;
            while (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        
        public IEnumerator FadeIn(float time) {
            canvasGroup.alpha = 1;
            while (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}