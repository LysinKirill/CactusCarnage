using UnityEngine;

namespace _Source.Game
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform _player;

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void LateUpdate()
        {
            var cameraTransform = transform;
            var temp = cameraTransform.position;
            var position = _player.position;
            temp.x = position.x;
            temp.y = position.y;
            cameraTransform.position = temp;
        }
    }
}
