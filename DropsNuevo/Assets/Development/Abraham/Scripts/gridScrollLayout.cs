using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gridScrollLayout : MonoBehaviour {

    public int maxHorizontalTags;       ///< maxHorizontalTags Valor maximo de tarjetas que contendra el grid layout
    public int scrollCount;             ///< scrollCount numero de tarjetas que avanzará con cada click en los botones
    public bool bandera = true;         ///< bandera bandera que verifica si ya se realizo el acomo inicial del grid scroll
    private GridLayoutGroup layout;     ///< layout layout que acomoda las tarjetas en forma de cuadricula
    private packManager[] hijos;        ///< hijos tarjetas dentro del layout
    private int count = 0;              ///< count valor inicial del hijo que debe mostrar, va desde 0 hasta (hijos.lenght - maxHorizontalTags)
    public bool isVertical;
    public bool estaAjustado = false;

    

    /**
     * Funcion que se manda llamar al inicio de la scena(frame 1)
     * se obtiene el componente layout del gameObject
     */
    void Start() {
        layout = gameObject.GetComponent<GridLayoutGroup>();
    }

    /**
     * Funcion que se llama cada frame
     * Verifica si la cantidad de hijos es mayor al valor maximo de 
     */
    private void Update() {
        if (isVertical) {
            if (!estaAjustado) {
                float hijosLength = gameObject.GetComponentsInChildren<packManager>().Length;
                float columnasCount = layout.constraintCount;
                int rowCount = (int)Math.Ceiling(hijosLength / columnasCount);
                if (rowCount <= 2) {
                    rowCount = 2;
                } else {
                    gameObject.GetComponent<RectTransform>().localPosition += new Vector3(0, (rowCount - 2) * -3.4f, 0);
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, (rowCount) * 6.58f);
                    estaAjustado = true;
                }
            }
        } else {
            hijos = gameObject.GetComponentsInChildren<packManager>(true);
            if (hijos.Length > maxHorizontalTags && bandera) {
                activarHijos();
                bandera = false;
            }
        }
    }


    #region Scroll horizontal

    /**
     * Funcion que se manda llamar cuando el usuario da click en el boton de scroll anterior
     * resta el numero de saltos al count en caso que sea diferente a 0
     */
    public void anterior() {
        if (count > 0) {
            count -= scrollCount;
        }
        activarHijos();
    }

    /**
     * Funcion que se manda llamar cuando el usuario da click en el boton de scroll siguiente
     * suma el numero de saltos al count en caso que sea diferente a (hijos.Length - maxHorizontalTags)
     */
    public void siguiente() {
        if (count < hijos.Length - maxHorizontalTags) {
            count += scrollCount;
        }
        activarHijos();
    }

    /**
     * Funcion que se manda llamar cada que se da click en un boton del scroll
     * Activa los objetos desde count hasta (count + maxHorizontalTags) y desactiva los restantes
     */
    void activarHijos() {
        List<GameObject> visibles = new List<GameObject>();
        List<GameObject> ocultos = new List<GameObject>();
        for (int i = 0; i < hijos.Length; i++) {
            if (i < count) {
                ocultos.Add(hijos[i].gameObject);
            } else if (i > count + (maxHorizontalTags - 1)) {
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
    #endregion
}
