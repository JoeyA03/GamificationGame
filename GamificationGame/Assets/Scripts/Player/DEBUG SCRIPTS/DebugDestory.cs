using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDestory : MonoBehaviour
{
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > 2f)
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        if(time > 3.5f)
        {
            Destroy(this.gameObject);
        }
        
    }
}
