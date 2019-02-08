using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    float dir = 90;

    // Update is called once per frame
    void Update(){
        transform.Rotate(Vector3.up * dir * Time.deltaTime);
    }

    public void cambio()
    {
        dir *= -1;
    }
}
