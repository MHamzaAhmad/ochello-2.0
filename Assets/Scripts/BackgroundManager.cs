using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject platformS;
    public GameObject hookS;

    private float platsMaxDistance = 8f;
    private float hookMaxHeight = 7f;
    private int noOFPlats = 3;
    private int noOFHooks = 3;

    public GameObject prevPlat;
    public GameObject preHook;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(platformAndHooksSpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator platformAndHooksSpawnRoutine()
    {
        while(true)
        {
            Vector2 pos2Create = new Vector2(prevPlat.transform.position.x + platsMaxDistance, prevPlat.transform.position.y);
            GameObject go = (GameObject) Instantiate(platformS,pos2Create , Quaternion.identity);
            go.transform.SetParent(this.transform);
            noOFPlats++;
            go.transform.name = "Platform " + noOFPlats;
            prevPlat = go;

            Vector2 pos2CreateHooks = new Vector2(preHook.transform.position.x + hookMaxHeight, preHook.transform.position.y);
            GameObject go1 = (GameObject)Instantiate(hookS, pos2CreateHooks, Quaternion.identity);
            go1.transform.SetParent(this.transform);
            noOFHooks++;
            go.transform.name = "Hook " + noOFHooks;
            preHook = go1;
            yield return new WaitForSeconds(2f);
        }
    }
}
