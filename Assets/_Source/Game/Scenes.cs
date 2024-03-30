using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Scenes : MonoBehaviour
    {
        public void ChangeScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }
    }
}

