using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationController : MonoBehaviour
{
    private static UINotificationController instance;
    public static UINotificationController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    /// <summary>
    /// text element for showing elapsed time
    /// </summary>
    public Text timeText,movesText;
    public int moves;
    private float timer;

    /// <summary>
    /// boolen for whether stopwatch running
    /// </summary>
    private bool timeRunning;


    public GameObject notificationImage;
    public Text notificationText;

    private string[] notifications = { "Drop the ring on a tower", "Dropping on smaller ring not allowed", "Pick the topmost ring" };

    private void Start()
    {
        timer = 0;
        moves = 0;
        timeRunning = false;
        notificationImage.SetActive(false);
    }

    void Update()
    {
        if (timeRunning)
        {
            timer += Time.deltaTime;
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");
            timeText.text = minutes + ":" + seconds;
        }

    }

    public void increaseMoves()
    {
        moves++;
        movesText.text = "" + moves;
    }

    public void showNotification(string value, int? notificationIndex=null)
    {
        notificationImage.SetActive(true);
        
        if (notificationIndex == null)
            notificationText.text = value;
        else
            notificationText.text = notifications[(int)notificationIndex];

        StartCoroutine(hideNotification());
    }
  

    public void changeTimerStatus(bool value)
    {
        timeRunning = value;
    }

    IEnumerator hideNotification()
    {  
        yield return new WaitForSeconds(3);
        notificationImage.SetActive(false);
    }
}
