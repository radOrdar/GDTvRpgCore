using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, ISaveable {
        [field: SerializeField] public float ExperiencePoints { get; private set; } = 0;
        
        public event Action onExperienceGained;

        public void GainExperience(float experience) {
            ExperiencePoints += experience;
            onExperienceGained?.Invoke(); 
        }

        public object CaptureState() {
            return ExperiencePoints;
        }

        public void RestoreState(object state) {
            print("Exp restore");
            ExperiencePoints = (float)state;
        }
    }
}