using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {
        private CanvasGroup canvasGroup;
        private Coroutine activeFadeRoutine;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time) {
            yield return StartCoroutine(Fade(1, time));
        }
        
        public IEnumerator FadeIn(float time) {
            yield return StartCoroutine(Fade(0, time));
        }
        
        private IEnumerator Fade(float target, float time) {
            if(activeFadeRoutine != null) StopCoroutine(activeFadeRoutine);
            activeFadeRoutine = StartCoroutine(FadeRoutine(target, time));
            yield return activeFadeRoutine;
        }

        private IEnumerator FadeRoutine(float alphaTarget, float time) {
            while (!Mathf.Approximately(alphaTarget, canvasGroup.alpha)) {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}