using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class appManager : MonoBehaviour {

    #region variables
    private string Nombre = "";
    private string PrimerApellido = "";
    private string SegundoApellido = "";
    private string Correo = "";
    private string Imagen = "";
    #endregion

    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public void setNombre(string Nombre) {
        this.Nombre = Nombre;
    }

    public void setPApellido(string PrimerApellido) {
        this.PrimerApellido = PrimerApellido;
    }

    public void setSApellido(string SegundoApellido) {
        this.SegundoApellido = SegundoApellido;
    }

    public void setCorreo(string Correo) {
        this.Correo = Correo;
    }

    public void setImagen(string Imagen) {
        this.Imagen = Imagen;
    }
}
