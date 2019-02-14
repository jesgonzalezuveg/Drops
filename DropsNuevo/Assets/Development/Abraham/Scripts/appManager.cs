using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class appManager : MonoBehaviour {
    
    private string Nombre = "";             ///< Nombre almacena el nombre del usuario que utiliza la aplicación
    private string Correo = "";             ///< Correo almacena el correo con el cual la cuenta esta vinculada
    private string Imagen = "";             ///< Imagen almacena la imagen, ya sea de facebook o bien de UVEG de la persona que utiliza la aplicación

    /**
     * Función que se llama antes de iniciar la escena
     * Se encarga de hacer que el objeto no se pueda destruir al mometo de cargar otra escena
     * 
     */ 
    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    /**
     * Asigna el valor del nombre de la persona
     * @param Nombre String que contiene el nombre del usuario
     */
    public void setNombre(string Nombre) {
        this.Nombre = Nombre;
    }


    /**
     * Asigna el valor del Correo de la persona
     * @param Correo String que contiene el Correo del usuario
     */
    public void setCorreo(string Correo) {
        this.Correo = Correo;
    }

    /**
     * Asigna el valor de la Imagen de la persona
     * @param Imagen String que contiene la Imagen del usuario
     */
    public void setImagen(string Imagen) {
        this.Imagen = Imagen;
    }

    /**
     * Regresa los datos del usuario correspondiente al Nombre
     * 
     */ 
    public string getNombre() {
        return Nombre;
    }


    /**
     * Regresa los datos del usuario correspondiente al correo
     * 
     */
    public string getCorreo() {
        return Correo;
    }

    /**
     * Regresa los datos del usuario correspondiente a la imagen
     * 
     */
    public string getImagen() {
        return Imagen;
    }
}
