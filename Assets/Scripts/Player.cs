using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
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
    public UIManager uiManager;

    private bool first = true;
    private bool second = false;
    private GameObject currHook;
    private bool canRotateEyes = true;
    Ray newRay;
    RaycastHit newHit;

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
        //startFindingHook();
        //hookFindingRoutine();

        rotateEyes();
        hookMaker();
    }

    private void hookFindingRoutine()
    {
        if (findHook)
        {
           ray = new Ray(eyes.transform.position, eyes.transform.right * 14);
           if (Physics.Raycast(ray, out hitInfo, 5.8f))
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
                uiManager.updateScores();
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
       currHook = (GameObject) Instantiate(ropeHook, eyes.transform.position, Quaternion.identity);

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

    private void rotateEyes()
    {
        if (canRotateEyes == true)
        {
            
            if (first == true)
            {
                eyes.transform.rotation = Quaternion.Lerp(eyes.transform.rotation,
                Quaternion.Euler(0f, 0f, 62f), Time.deltaTime * 10f);
                if (eyes.transform.rotation == Quaternion.Euler(0f, 0f, 62f))
                {
                    second = true;
                    first = false;
                }
            }
            else if (second == true)
            {
                eyes.transform.rotation = Quaternion.Lerp(eyes.transform.rotation,
                Quaternion.Euler(0f, 0f, -36f), Time.deltaTime * 10f);
                if (eyes.transform.rotation == Quaternion.Euler(0f, 0f, -36f))
                {
                    second = false;
                    first = true;
                }
            }
        }
        Debug.DrawLine(eyes.transform.position, eyes.transform.right * 30, Color.cyan);
        newRay = new Ray(eyes.transform.position, eyes.transform.right * 20);

    }

    private void hookMaker()
    {
        if (Input.GetMouseButton(0))
        {
            canRotateEyes = false;
            if (canThrowHook == false)
                throwHook();
            currHook.GetComponent<RopeScript>().moving = true;


            Vector2 direction;
            if (Physics.Raycast(newRay, out newHit, 10))
            {
                Debug.DrawRay(newRay.origin, newRay.direction, Color.grey);
                direction = newRay.direction;
            }
            else
            {
                direction = newRay.direction;
                Debug.DrawRay(newRay.origin, newRay.direction, Color.blue);
            }
            //Vector3 direction = eyes.transform.right * 40;
            direction.Normalize();
            currHook.transform.Translate(direction * 5 * Time.deltaTime);
        }
        if (Input.GetMouseButtonUp(0))
        {
            currHook.GetComponent<RopeScript>().moving = false;
            currHook.GetComponent<Rigidbody2D>().isKinematic = false;
            
        }
    }
}
