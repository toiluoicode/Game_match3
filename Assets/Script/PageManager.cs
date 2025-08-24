using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ExitApp()
    {
        Application.Quit();
    }
    public void LoadScenPlay()
    {
        SceneManager.LoadScene("Play_Scene");
    }
    public void LoadSceneMain()
    {
        SceneManager.LoadScene("Menu");
    }
}
