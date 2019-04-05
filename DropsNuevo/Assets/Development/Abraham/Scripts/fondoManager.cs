using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fondoManager : MonoBehaviour {

    public Texture[] textures;
    /*public Texture[] noche;
    public Texture[] tarde;
    public Texture[] dia;*/

    appManager manager;

    public void Start() {
        manager = GameObject.FindObjectOfType<appManager>();
        /*switch (System.DateTime.Now.TimeOfDay.Hours) {
            case int n when (n >= 20 || n <= 5):
                Debug.Log(System.DateTime.Now.TimeOfDay.Hours);
                Debug.Log("caso 1");
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", noche[manager.fondo]);
                break;
            case int n when (n >= 6 && n <= 11):
                Debug.Log(System.DateTime.Now.TimeOfDay.Hours);
                Debug.Log("caso 2");
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", dia[manager.fondo]);
                break;
            case int n when (n >= 12 && n <= 19):
                Debug.Log(System.DateTime.Now.TimeOfDay.Hours);
                Debug.Log("caso 3");
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", tarde[manager.fondo]);
                break;
        }*/
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", textures[manager.fondo]);
    }


}
