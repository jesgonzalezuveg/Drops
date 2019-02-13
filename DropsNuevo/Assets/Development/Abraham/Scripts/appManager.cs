using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class appManager : MonoBehaviour {

    #region variables
    public static string Nombre = "";
    public static string PrimerApellido = "";
    public static string SegundoApellido = "";
    public static string Correo = "";
    public static string Imagen = "";

    #endregion
    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }


}
