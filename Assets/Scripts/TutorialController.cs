using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    private static TutorialController instance;
    public static TutorialController Instance
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

    int diskIndex=0;
    public void startTutorial()
    {
        diskIndex++;
        if (diskIndex == 1)
        {
            moveDisk(DiskController.Instance.disk[0], 1);
            UINotificationController.Instance.showNotification("Only One Disc Can be Moved At a time",null);
        }

        if (diskIndex == 2)
        {
            moveDisk(DiskController.Instance.disk[1], 2);
            UINotificationController.Instance.showNotification("Each move consists of moving topmost Disc", null);
            UINotificationController.Instance.increaseMoves();
        }

        if (diskIndex == 3)
        {
            moveDisk(DiskController.Instance.disk[2], 1);
            UINotificationController.Instance.showNotification("Larger diameter disc can never be placed on smaller disc", null);
            UINotificationController.Instance.increaseMoves();
        }
    }

    private void moveDisk(GameObject disk,int destinationTower)
    {
        disk.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(moveUp(disk,destinationTower));
    }

    private void showText(string text)
    {

    }

    IEnumerator moveUp(GameObject disk, int destinationTower)
    {
        while (disk.transform.position.y < 3f)
        {
            disk.transform.position = new Vector3(disk.transform.position.x,disk.transform.position.y+0.01f,disk.transform.position.z);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(moveHorizontal(disk,destinationTower)); 
    }

    IEnumerator moveHorizontal(GameObject disk, int destinationTower)
    {
        Debug.Log("horizontal started");
        float targetPosX = DiskController.Instance.towers[destinationTower].transform.position.x;
        while (disk.transform.position.x < targetPosX)
        {
            Debug.Log("moving horizontally");
            disk.transform.position = new Vector3(disk.transform.position.x+0.03f, disk.transform.position.y, disk.transform.position.z);
            yield return new WaitForEndOfFrame();
        }
        if (diskIndex == 3)
        {
            disk.transform.position = new Vector3(-2f, disk.transform.position.y, disk.transform.position.z);
            disk.GetComponent<Rigidbody>().useGravity = true;
            SceneManager.LoadScene(0);
        }
        else
        {
            disk.GetComponent<Rigidbody>().useGravity = true;
            startTutorial();
        }
    }

    IEnumerator hideTextAfterSecond()
    {

        yield return new WaitForEndOfFrame();
    }
}
