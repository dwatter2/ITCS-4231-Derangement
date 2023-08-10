using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    // Update is called once per frame
    void Update()
    {
        if(Camera.main)
        {
        Vector3 target = new Vector3(Camera.main.transform.position.x, 
        this.transform.position.y, Camera.main.transform.position.z);
        
        transform.LookAt(target);
        }
    }
}
