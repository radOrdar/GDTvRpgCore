using System;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {
        [SerializeField] private float radius = .3f;

        private void OnDrawGizmos() {
            foreach (Transform t in transform) {
                Gizmos.DrawSphere(t.position, radius);
            }
        }
    }
}