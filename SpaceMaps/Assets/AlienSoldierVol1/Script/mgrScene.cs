using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mgrScene : MonoBehaviour {

    int sceneNum = 0;
    bool isTotal;

    void Start()
    {
        SceneManager.LoadScene("Total", LoadSceneMode.Additive);
    }

    public void ShooterScLoadAct()
    {
        if (sceneNum > 2)
        {
            sceneNum = 0;
            SceneManager.UnloadSceneAsync("Launcher");
        }
        if (sceneNum == 0)
        {
            if (!isTotal)
            {
                SceneManager.UnloadSceneAsync("Total");
                isTotal = true;
            }
            
            SceneManager.LoadScene("Shooter", LoadSceneMode.Additive);
        }
        if (sceneNum == 1)
        {
            SceneManager.UnloadSceneAsync("Shooter");
            SceneManager.LoadScene("Melee", LoadSceneMode.Additive);
        }
        if (sceneNum == 2)
        {
            SceneManager.UnloadSceneAsync("Melee");
            SceneManager.LoadScene("Launcher", LoadSceneMode.Additive);
        }

        sceneNum++;
    }
}
