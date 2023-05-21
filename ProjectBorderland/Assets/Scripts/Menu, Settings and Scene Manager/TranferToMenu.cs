using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class TranferToMenu : MonoBehaviour
{
    SceneHandler sceneHandler = new SceneHandler();

    private void OnEnable()
    {
        sceneHandler.LoadNextScene();
    }
}
