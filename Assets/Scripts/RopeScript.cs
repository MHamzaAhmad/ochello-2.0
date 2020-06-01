using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector3 hookDest;
    public Vector3 playerPos;
    private float speed = 0.3f;
    private float segLength = 0.4f;
    public GameObject node;
    List<GameObject> nodes = new List<GameObject>();
    private GameObject lastNode;

    private LineRenderer lineRenderer;
    private float lineWidth = 0.15f;

    private GameObject player;
    private bool playerConnected = true;
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
    }

    private void FixedUpdate()
    {
        //hookThrower();
    }

    private void hookThrower()
    {
        transform.position = Vector3.MoveTowards(transform.position, hookDest, speed);

        if (this.transform.position != hookDest)
        {
            if (Vector3.Distance(playerPos, lastNode.transform.position) > segLength)
            {
                createNode();
                playerConnected = false;
            }
        }
        else if (Vector3.Distance(playerPos, lastNode.transform.position) > segLength)
        {
            //while (Vector3.Distance(playerPos, lastNode.transform.position) > segLength)
            //{
            //    createNode();
            //}
        }
        if (playerConnected == false)
        {
            playerConnected = true;
            Debug.Log("player connected");
            //player.GetComponent<SpringJoint>().connectedBody = lastNode.GetComponent<Rigidbody>();
            lastNode.GetComponent<SpringJoint>().connectedBody = player.GetComponent<Rigidbody>();
        }
        //canSwing = true;
    }

    private void createNode()
    {
        Vector3 pos2Create = playerPos - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= segLength;
        pos2Create += lastNode.transform.position;

        GameObject go = (GameObject) Instantiate(node, pos2Create, Quaternion.identity);
        go.transform.SetParent(this.transform);
        go.GetComponent<SpringJoint>().connectedBody = lastNode.GetComponent<Rigidbody>();
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

    
}
