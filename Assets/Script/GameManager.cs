using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image MuteImage;        
    [SerializeField] private Sprite muteImage;        
    [SerializeField] private Sprite unMuteImage;      
    //Pause Menu
    [SerializeField] private GameObject Board;
    public GameObject pauseMenu;
    bool isPause = false;
    private bool muteAudio = false;
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MuteAudio()
    {
        muteAudio = !muteAudio;
        audioSource.mute = muteAudio;
        if (muteAudio == false)
        {

            MuteImage.sprite = muteImage;
        }
        else
        {
            MuteImage.sprite = unMuteImage;
        }
    }
    public void PauseMenu()
    {
        pauseMenu.gameObject.SetActive(true);
        this.Board.gameObject.SetActive(false);
        Time.timeScale = 0f;
        isPause = true;
    }
    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
        this.Board.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isPause = false;
    }

}
