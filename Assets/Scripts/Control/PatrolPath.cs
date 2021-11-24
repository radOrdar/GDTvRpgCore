using System;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {
        [SerializeField] private float radius = .3f;

        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++) {
                Gizmos.DrawSphere(GetWaypoint(i), radius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(i + 1));
            }
        }

        private Vector3 GetWaypoint(int i) {
            i = i == transform.childCount ? 0 : i;
            return transform.GetChild(i).position;
        }
    }
}