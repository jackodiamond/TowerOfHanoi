using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    public GameObject playButton, tutorialButton;

    public int moves;

    public void startGame()
    {
        playButton.SetActive(false);
        tutorialButton.SetActive(false);
        DiskController.Instance.allowMoving = true;
        UINotificationController.Instance.changeTimerStatus(true);
    }

    public void showTutorial()
    {
        playButton.SetActive(false);
        tutorialButton.SetActive(false);
        TutorialController.Instance.startTutorial();
        UINotificationController.Instance.changeTimerStatus(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(0);
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}
