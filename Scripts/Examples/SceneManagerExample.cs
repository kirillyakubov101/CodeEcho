using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeEcho.Samples
{
    public class SceneManagerExample : MonoBehaviour, ICodeEcho
    {
        [CodeEchoMark("LoadScene")]
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}

