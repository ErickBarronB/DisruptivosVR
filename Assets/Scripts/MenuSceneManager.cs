using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{


public void LoadScene(string sceneName)
{
    SceneManager.LoadScene(sceneName);
}


public void PrintDebug(string message)
{
    Debug.Log(message);
}
}
