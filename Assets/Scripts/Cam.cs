using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject target;
    public int type;
    public float smoothTime = 0.3f;    // Damping time
    private Vector3 velocity = Vector3.zero;
    Vector3 targetTransform;
    public float yOffset = 1;

    private bool camLock = false;

    public float maxHeight = 99999;

    // Update is called once per frame
    private void Start()
    {


    }
    void Update()
    {
        //if (Math.Abs(target.GetComponent<Rigidbody2D>().linearVelocity.y) > minYspeed)
        //{
        targetTransform = target.transform.position;
        targetTransform.z = transform.position.z;
        targetTransform.y = target.transform.position.y + yOffset;

        //if(transform.position.y + (GetComponent<Camera>().orthographicSize / 2) < maxHeight)
        transform.position = Vector3.SmoothDamp(transform.position, targetTransform, ref velocity, smoothTime);

        if(transform.position.y + (GetComponent<Camera>().orthographicSize / 2) > maxHeight || camLock)
        {
            camLock = true;
            //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y - 3, transform.position.z), ref velocity, smoothTime);
            transform.position = new Vector3(transform.position.x, 1.07f, transform.position.z);
            transform.GetComponent<Camera>().orthographicSize = 6.93f;
        }


    }
        // else
        // {
        //     transform.position = new Vector3(target.transform.position.x, yspot, -10);
        // }
        // }
}
