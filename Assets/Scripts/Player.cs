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

    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray(eyes.transform.position, eyes.transform.right);
        rigidbody2d = GetComponent<Rigidbody2D>();
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
           if (Physics.Raycast(ray, out hitInfo, 100))
           {
                Debug.DrawLine(ray.origin, eyes.transform.right * 14, Color.red);
                if (hitInfo.collider.tag == "Hook")
                {
                    findHook = false;
                    Debug.DrawLine(ray.origin, hitInfo.transform.position, Color.yellow);
                    ropeHook.GetComponent<RopeScript>().hookDest = (Vector2) hitInfo.transform.position;
                    ropeHook.GetComponent<RopeScript>().playerPos = (Vector2)this.transform.position;
                    ropeHook.GetComponent<RopeScript>().hook = hitInfo.transform.gameObject;
                    throwHook();
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
            findHook = true;
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
        rigidbody2d.AddForce(Vector2.right * 900);
    }

}
