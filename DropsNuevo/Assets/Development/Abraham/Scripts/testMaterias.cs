using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testMaterias : MonoBehaviour {

    GameObject manager;
    public Image imagen;

    private void Awake() {
        manager = GameObject.Find("AppManager");
    }

    IEnumerator Start() {
        WWW www = new WWW(manager.GetComponent<appManager>().getImagen());
        yield return www;
        imagen.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

}
