using RPG.Saving;
using UnityEngine;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, ISaveable {
        [field: SerializeField] public float ExperiencePoints { get; private set; } = 0;

        public void GainExperience(float experience) {
            ExperiencePoints += experience;
        }

        public object CaptureState() {
            return ExperiencePoints;
        }

        public void RestoreState(object state) {
            ExperiencePoints = (float)state;
        }
    }
}