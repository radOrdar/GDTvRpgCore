using UnityEngine;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;

        private void Update() {
            if (Mathf.Approximately(0f, health.GetPercentage()) ||
                Mathf.Approximately(100f, health.GetPercentage())) {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(health.GetPercentage()/100,1,1);
        }
    }
}