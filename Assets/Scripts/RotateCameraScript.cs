using UnityEngine;

public class RotateCameraScript : MonoBehaviour
{
    private static RotateCameraScript instance;
    public static RotateCameraScript Instance
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

    public float speed = 3.5f;
    public bool rotationActive=true;
    private float X;
    private float Y;

    void Update()
    {
        if (Input.GetMouseButton(0)&&rotationActive)
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }
    }
}

