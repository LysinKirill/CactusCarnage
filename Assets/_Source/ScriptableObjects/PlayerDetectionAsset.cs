using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerDetectionAsset", menuName = "SO/newPlayerDetectionAsset")]
    public class PlayerDetectionAsset : ScriptableObject
    {
        public bool PlayerDetected
        {
            get { return _playerDetected; }
            set
            {
                _playerDetected = value;
                if (_playerDetected)
                    OnPlayerSpotted?.Invoke();
            }
        }

        [field: SerializeField] public bool LineOfSightRequired { get; private set; }
        [field: SerializeField] public float DetectionRadius { get; private set; }

        private bool _playerDetected = false;

        public event Action OnPlayerSpotted;
    }
}
