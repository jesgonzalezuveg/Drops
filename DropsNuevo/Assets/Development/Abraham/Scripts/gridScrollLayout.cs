using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gridScrollLayout : MonoBehaviour {

    public int maxHorizontalTags;
    public int scrollCount;
    public bool bandera = true;
    private GridLayoutGroup layout;
    private packManager[] hijos;
    private int count = 0;

    void Start() {
        layout = gameObject.GetComponent<GridLayoutGroup>();
    }

    void Update() {
        hijos = gameObject.GetComponentsInChildren<packManager>(true);
        if (hijos.Length > maxHorizontalTags && bandera) {
            activarHijos();
            bandera = false;
        }
    }

    public void anterior() {
        if (count > 0) {
            count -= scrollCount;
        }
        activarHijos();
    }

    public void siguiente() {
        if (count < hijos.Length - maxHorizontalTags) {
            count += scrollCount;
        }
        activarHijos();
    }

    void activarHijos() {
        List<GameObject> visibles = new List<GameObject>();
        List<GameObject> ocultos = new List<GameObject>();
        for (int i = 0; i < hijos.Length; i++) {
            if (i < count) {
                ocultos.Add(hijos[i].gameObject);
            }else if (i > count + (maxHorizontalTags - 1)) {
                ocultos.Add(hijos[i].gameObject);
            } else {
                visibles.Add(hijos[i].gameObject);
            }
        }
        foreach (var visible in visibles) {
            visible.SetActive(true);
        }
        foreach (var oculto in ocultos) {
            oculto.SetActive(false);
        }
    }
}
