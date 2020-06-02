using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 hookDest;
    public Vector2 playerPos;
    public GameObject hook;
    private float speed = 1f;
    private float segLength = 0.25f;
    public GameObject node;
    List<GameObject> nodes = new List<GameObject>();
    private GameObject lastNode;
    private bool nodeCreation = false;

    private LineRenderer lineRenderer;
    private float lineWidth = 0.15f;

    private GameObject player;
    private bool playerConnected = false;
    // Start is called before the first frame update
    void Start()
    {
        nodes.Add(this.gameObject);
        lastNode = this.gameObject;
        lineRenderer = GetComponent<LineRenderer>();
        if (player == null)
        {
            Debug.LogError("Player in ropeScript is null.");
            player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        hookThrower();
        renderLine();
        //player.transform.Translate(1f, 0f, 0f);
        //this.transform.Rotate(0f, 1f, 0f);
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.forward);
        }

        
    }

    private void FixedUpdate()
    {
        //hookThrower();
        constraints();
    }

    private void hookThrower()
    {
        transform.position = Vector3.MoveTowards(transform.position, hookDest, speed);

        if ((Vector2) this.transform.position != hookDest)
        {
            if (Vector2.Distance(playerPos, lastNode.transform.position) > segLength)
            {
                createNode();
                //playerConnected = false;
            }
        }
        else //if (Vector3.Distance(playerPos, lastNode.transform.position) > segLength)
        {
            //while (Vector3.Distance(playerPos, lastNode.transform.position) > segLength)
            //{
            //    createNode();
            //}
            if (playerConnected == false)
            {
                playerConnected = true;
                Debug.Log("player connected");
                while (Vector2.Distance(playerPos, lastNode.transform.position) > segLength)
                {
                    createNode();
                    Debug.Log("node created in while");
                }
                //lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
                player.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
                //lastNode.GetComponent<HingeJoint>().maxDistance = 0;
                //this.GetComponent<HingeJoint2D>().connectedBody = hook.GetComponent<Rigidbody2D>();
            }
        }
    }

    private void createNode()
    {
        Vector2 pos2Create = playerPos - (Vector2)lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= segLength;
        pos2Create += (Vector2) lastNode.transform.position;
        
        GameObject go = (GameObject) Instantiate(node, pos2Create, Quaternion.identity);
        go.transform.SetParent(this.transform);
        go.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
        nodes.Add(go);
        lastNode = go;
    }

    private void renderLine()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = nodes.Count;

        for (int i = 0; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(i, nodes[i].transform.position);
        }
    }

    private void constraints()
    {
        //for (int i = 0; i < nodes.Count - 1; i++)
        //{
        //    GameObject firstseg = nodes[i];
        //    GameObject secondseg = nodes[i + 1];

        //    float dist = (firstseg.transform.position - secondseg.transform.position).magnitude;
        //    float error = Mathf.Abs(dist - this.segLength);
        //    Vector3 changedir = Vector3.zero;

        //    if (dist > segLength)
        //    {
        //        changedir = (firstseg.transform.position - secondseg.transform.position).normalized;
        //    }
        //    else if (dist < segLength)
        //    {
        //        changedir = (secondseg.transform.position - firstseg.transform.position).normalized;
        //    }

        //    Vector3 changeamount = changedir * error;

        //    if (i != 0)
        //    {
        //        firstseg.transform.position -= changeamount * error;
        //        this.nodes[i] = firstseg;
        //        secondseg.transform.position += changeamount * 0.5f;
        //        this.nodes[i + 1] = secondseg;
        //    }
        //    else
        //    {
        //        secondseg.transform.position += changeamount;
        //        this.nodes[i + 1] = secondseg;
        //    }
        //}

        nodes[nodes.Count - 1].transform.position = player.transform.position;
        this.transform.position = hookDest;
    }
}
