using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPic1;
    [SerializeField] GameObject menuPic2;
    [SerializeField] int picNum = 1;
    [SerializeField] bool isAnimating = false;
    [SerializeField] GameObject fadeOut;
    [SerializeField] AudioSource buttonClick;
    [SerializeField] int sceneToLoad;
    [SerializeField] int saveTransferValue;
    [SerializeField] GameObject fadeIn;

    void Start()
    {
        StartCoroutine(StopFade());
    }

    public void StartGame()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        StartCoroutine(TransferToClassScene());
    }

    public void LoadGame()
    {
        saveTransferValue = PlayerPrefs.GetInt("LoadState");
        if (saveTransferValue > 0)
        {
            sceneToLoad = saveTransferValue + 1;
            buttonClick.Play();
            fadeOut.SetActive(true);
            StartCoroutine(LoadScene());
        }
    }

    public void GotToCredits()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        StartCoroutine(TransferToCredits());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (isAnimating == false)
        {
            isAnimating = true;
            StartCoroutine(RandomPic());
        }
    }

    IEnumerator RandomPic()
    {
        yield return new WaitForSeconds(1);
        picNum = Random.Range(1, 3);  // Changed from 1,4 to 1,3
        if (picNum == 1)
        {
            menuPic1.SetActive(true);
            menuPic2.SetActive(false);
        }
        else if (picNum == 2)  // Changed from if (picNum == 2) block
        {
            menuPic1.SetActive(false);
            menuPic2.SetActive(true);
        }
        yield return new WaitForSeconds(7);
        menuPic1.SetActive(false);
        menuPic2.SetActive(false);
        isAnimating = false;
    }

    IEnumerator TransferToClassScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(5);
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator TransferToCredits()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(4);
    }

    IEnumerator StopFade()
    {
        yield return new WaitForSeconds(2);
        fadeIn.SetActive(false);
    }
}