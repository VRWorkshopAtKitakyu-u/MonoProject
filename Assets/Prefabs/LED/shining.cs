using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shining : MonoBehaviour
{
    public Material colorA;
    public Material colorB;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      GetComponent<Renderer>().material.color=colorB.color;
      if(Input.GetKey(KeyCode.Space)){
        GetComponent<Renderer>().material.color=colorA.color;
      }  
    }
}
