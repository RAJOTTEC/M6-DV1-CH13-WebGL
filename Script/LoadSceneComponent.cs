using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadSceneComponent : MonoBehaviour 
{
    float timer = 0;
    public string loadThisScene;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            SceneManager.LoadScene(loadThisScene);
        }
    }
}