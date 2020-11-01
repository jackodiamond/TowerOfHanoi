using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// script for controlling the disks
/// </summary>
public class DiskController : MonoBehaviour
{
    private static DiskController instance;
    public static DiskController Instance
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

    //use three stacks for rings placed
    public Stack[] towerStack = new Stack[3];
    public GameObject[] towers;
    public GameObject[] disk;
    public bool allowMoving = false;
    private bool diskDragging;
    private GameObject currentDisk;
    private int currentTower;
    bool topdisk;

    // Start is called before the first frame update
    void Start()
    {
        diskDragging = false;
        topdisk = false;
        towerStack[0] = new Stack();
        for (int i = 6; i >= 0; i--)
        {
            towerStack[0].Push(disk[i]);
        }
        towerStack[1] = new Stack();
        towerStack[2] = new Stack();

    }

    void Update()
    {
        if (allowMoving)
        {
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "disk")
                    {
                        RotateCameraScript.Instance.rotationActive = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (towerStack[i].Count != 0)
                                if (hit.transform.gameObject == (GameObject)towerStack[i].Peek())
                                {
                                    topdisk = true;
                                    towerStack[i].Pop();
                                    currentTower = i;

                                }
                        }
                        if (topdisk)
                        {
                            diskDragging = true;
                            currentDisk = hit.transform.gameObject;
                        }
                        else
                        {
                            UINotificationController.Instance.showNotification("", 2);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                dropDisk(currentDisk);
                diskDragging = false;
                topdisk = false;
                currentDisk = null;
                RotateCameraScript.Instance.rotationActive = true;
            }

            if (diskDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 mousePos = ray.GetPoint(10);
                //currentDisk.transform.position = new Vector3(mousePos.x,mousePos.y, currentDisk.transform.position.z);
                //     currentDisk.GetComponent<Rigidbody>().MovePosition(new Vector3(mousePos.x, mousePos.y, currentDisk.transform.position.z));


                // calc velocity necessary to follow the mouse pointer
                Vector3 velocity = new Vector3(mousePos.x, mousePos.y, 0f) - new Vector3(currentDisk.transform.position.x, currentDisk.transform.position.y, 0f);


                if (velocity.magnitude < 0.1f)
                    velocity = Vector3.zero;
                else
                    velocity = Vector3.Normalize(velocity);

                currentDisk.GetComponent<Rigidbody>().useGravity = false;
                currentDisk.GetComponent<Rigidbody>().velocity = velocity * 4f;
            }
        }

    }


    private void dropDisk(GameObject disk)
    {
        if (disk == null)
            return;

        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(disk.transform.position.x - towers[i].transform.position.x) < 0.5f)
            {
                if (isDropSafe(i))
                {
                    disk.transform.position = new Vector3(towers[i].transform.position.x, disk.transform.position.y, towers[i].transform.position.z);
                    towerStack[i].Push(currentDisk);
                    currentDisk.GetComponent<Rigidbody>().useGravity = true;
                    checkWinningCondition();
                    UINotificationController.Instance.increaseMoves();
                    return;
                }
                else
                {
                    disk.transform.position = new Vector3(towers[currentTower].transform.position.x, 2.6f, towers[currentTower].transform.position.z);
                    towerStack[currentTower].Push(currentDisk);
                    currentDisk.GetComponent<Rigidbody>().useGravity = true;
                    UINotificationController.Instance.showNotification("", 1);
                    return;
                }
            }
        }
        currentDisk.GetComponent<Rigidbody>().useGravity = true;
        disk.transform.position = new Vector3(towers[currentTower].transform.position.x, 2.6f, towers[currentTower].transform.position.z);
        towerStack[currentTower].Push(currentDisk);
        UINotificationController.Instance.showNotification("", 0);
        return;
    }

    private bool isDropSafe(int droppingTower)
    {
        if (droppingTower == currentTower)
            return true;

        if (towerStack[droppingTower].Count == 0)
            return true;

        GameObject topDisk = (GameObject)towerStack[droppingTower].Peek();
        if (topDisk.GetComponent<DiskObject>().index > currentDisk.GetComponent<DiskObject>().index)
        {
            return true;
        }

        return false;
        //check from stack
    }

    private void checkWinningCondition()
    {
        if (towerStack[1].Count == 7|| towerStack[2].Count==7)
        {
            allowMoving = false;
            UINotificationController.Instance.showNotification("You Won", null);
            UINotificationController.Instance.changeTimerStatus(false);
        }
    }
}
