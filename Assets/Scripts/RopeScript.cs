using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 hookDest;
    public Vector2 playerPos;
    public GameObject hook;
    private float speed = 1.5f;
    private float segLength = 0.25f;
    private float nodesDeletionSpeed = 0.3f;
    public GameObject node;
    public List<GameObject> nodes = new List<GameObject>();
    private GameObject lastNode;
    private bool nodeCreation = true;

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
            player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        hookThrower();
        renderLine();
        deleteNodes();
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("d key presssed");
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("player not giving rigid body");
            }
            else
                Debug.Log("d: rigid body i not null");
            player.GetComponent<Player>().littlePush();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            player.GetComponent<Player>().detach();
        }

        
    }

    private void FixedUpdate()
    {
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
            }
        }
        else 
        {
            if (playerConnected == false)
            {
                playerConnected = true;
                Debug.Log("player connected");
                while (Vector2.Distance(playerPos, lastNode.transform.position) > segLength)
                {
                    createNode();
                    Debug.Log("node created in while");
                }
                player.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
                nodeCreation = false;
                deleteNodes();
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
        this.transform.position = hookDest;
    }

    private void deleteNodes()
    {
        if (nodeCreation == false)
        {
            int deleteNodes =0;
            int startingNode = 2;
            int toBeDeleted =0;
            float totalNoOfNodes = Vector2.Distance(player.transform.position, hookDest) / segLength;
            totalNoOfNodes = Mathf.Floor(totalNoOfNodes);
            if (totalNoOfNodes < nodes.Count)
            {
                deleteNodes = nodes.Count - (int) totalNoOfNodes;
                deleteNodes += 3;
            }
            else
            {
                deleteNodes = 2;
            }
            toBeDeleted = startingNode + deleteNodes + 1;
            Debug.Log("deleting nodes");
            nodeCreation = true;
            GameObject firstNode = nodes[startingNode -1];
            GameObject secondNode = nodes[toBeDeleted];
            firstNode.transform.name = "firstnode";
            secondNode.transform.name = "secondNode";
            secondNode.GetComponent<HingeJoint2D>().connectedBody = firstNode.GetComponent<Rigidbody2D>();
            for (int i = startingNode; i < toBeDeleted; i++)
            {
                GameObject tempNode = nodes[startingNode];
                Destroy(tempNode);
                nodes.RemoveAt(startingNode);
            }
            for (int i = startingNode; i < nodes.Count; i++)
            {
                while (Vector2.Distance(nodes[i - 1].transform.position, nodes[i].transform.position) > segLength)
                {
                    Debug.Log("moving towards");
                    nodes[i].transform.position = Vector2.MoveTowards(nodes[i].transform.position, nodes[i -1].transform.position, nodesDeletionSpeed);
                }
            }
            nodes[startingNode -1] = firstNode;
            nodes[startingNode] = secondNode;
            while (Vector2.Distance(nodes[nodes.Count -1].transform.position, player.transform.position) > segLength)
            {
                Debug.Log("player moving towards");
                player.transform.position = Vector2.MoveTowards(player.transform.position, nodes[nodes.Count -1].transform.position, nodesDeletionSpeed);
            }
            lineRenderer.positionCount = nodes.Count;
            for (int i = startingNode; i < nodes.Count; i++)
            {
                lineRenderer.SetPosition(i, nodes[i].transform.position);
            }
        }
    }
}
