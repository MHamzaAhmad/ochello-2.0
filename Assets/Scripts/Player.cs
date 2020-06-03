using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject eyes;
    private Ray ray;
    private RaycastHit hitInfo;
    private bool findHook = false;
    public GameObject ropeHook;
    private bool canThrowHook = false;
    private Rigidbody2D rigidbody2d;
    private string currPlatformName;
    private Quaternion eyesTPose;
    private GameObject prevHook;

    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray(eyes.transform.position, eyes.transform.right);
        rigidbody2d = GetComponent<Rigidbody2D>();
        eyesTPose = eyes.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        startFindingHook();
        hookFindingRoutine();
        if (Input.GetMouseButtonDown(1))
        {
            this.GetComponent<Rigidbody>().AddForce(4f, 0f, 0f);
        }

    }

    private void hookFindingRoutine()
    {
        if (findHook)
        {
           ray = new Ray(eyes.transform.position, eyes.transform.right * 14);
           if (Physics.Raycast(ray, out hitInfo, 7))
           {
                Debug.DrawLine(ray.origin, eyes.transform.right * 14, Color.red);
                if (hitInfo.collider.tag == "Hook")
                {
                    if (hitInfo.transform.gameObject != prevHook || prevHook == null)
                    {
                        findHook = false;
                        Debug.DrawLine(ray.origin, hitInfo.transform.position, Color.yellow);
                        ropeHook.GetComponent<RopeScript>().hookDest = (Vector2)hitInfo.transform.position;
                        ropeHook.GetComponent<RopeScript>().playerPos = (Vector2)this.transform.position;
                        ropeHook.GetComponent<RopeScript>().hook = hitInfo.transform.gameObject;
                        throwHook();
                        prevHook = hitInfo.transform.gameObject;
                    }
                    else
                    {
                        eyes.transform.Rotate(0f, 0f, 2f);
                    }
                }
                else
                {
                    Debug.DrawLine(ray.origin, eyes.transform.right * 14, Color.green);
                    eyes.transform.Rotate(0f, 0f, 2f);
                }
           }
           else
           {
                Debug.DrawLine(ray.origin, eyes.transform.right * 14, Color.blue);
                eyes.transform.Rotate(0f, 0f, 2f);
           }
        }
    }

    private void startFindingHook()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(canThrowHook == false)
            findHook = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            if (collision.transform.name != currPlatformName)
            {
                currPlatformName = collision.transform.name;
                canThrowHook = false;
                this.GetComponent<HingeJoint2D>().enabled = true;
            }
            else if(currPlatformName == null)
            currPlatformName = collision.transform.name;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            eyes.transform.rotation = eyesTPose;
            Debug.Log("Exit platform");
        }
    }

    private void throwHook()
    {
        if(canThrowHook == false)
        Instantiate(ropeHook, this.transform.position, Quaternion.identity);

        canThrowHook = true;
    }

    public void littlePush()
    {
        rigidbody2d.AddForce(Vector2.right * 200);
    }

    public void detach()
    {
        this.GetComponent<HingeJoint2D>().connectedBody = null;
        this.GetComponent<HingeJoint2D>().enabled = false;
    }

}
