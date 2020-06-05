using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class SimplePlatforms : MonoBehaviour
{
    public GameObject personalHook;
    private Vector2 hookPos;
    private float hookDistance = 4f;
    private float hookMinDist = 1.5f;
    private float hookHeight = 6f;
    private float hookMinHeight = 5f;
    private float percentage = 0.08f;
    // Start is called before the first frame update
    void Start()
    {
        hookSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void hookSpawn()
    {
        float height = Random.Range(hookMinHeight, hookHeight);
        float APheight = hookHeight - height;
        float distance = APheight * percentage;
        hookPos = new Vector2(this.transform.position.x + Random.Range(hookMinDist + distance, hookDistance - distance), this.transform.position.y + height);
        GameObject go = (GameObject)Instantiate(personalHook, hookPos, Quaternion.identity);
        go.transform.SetParent(this.transform);
    }
}
