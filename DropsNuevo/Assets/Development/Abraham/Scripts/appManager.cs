using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class appManager : MonoBehaviour {

    private string Usuario = "";            ///< Usuario almacena el usuario que utiliza la aplicación
    private string Nombre = "";             ///< Nombre almacena el nombre del usuario que utiliza la aplicación
    private string Correo = "";             ///< Correo almacena el correo con el cual la cuenta esta vinculada
    private string Imagen = "";             ///< Imagen almacena la imagen, ya sea de facebook o bien de UVEG de la persona que utiliza la aplicación
    private webServicePaquetes.paqueteData[] paquetes = null;
    private bool banderaPaquetes = true;
    private webServiceCategoria.categoriaData[] categorias = null;
    private bool banderaCategorias = true;
    private webServiceMateria.materiaData[] materias = null;
    private bool banderaMaterias = true;
    private webServiceEjercicio.ejercicioData[] ejercicios = null;
    private bool banderaEjercicios = true;
    private webServicePreguntas.preguntaData[] preguntas = null;
    private bool banderaPreguntas = true;

    #region setter y getters
    /**
     * Asigna el valor del usuario
     * @param Usuario String que contiene el usuario
     */
    public void setUsuario(string Usuario) {
        this.Usuario = Usuario;
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
    #endregion

    public void setPaquetes(webServicePaquetes.paqueteData[] pack) {
        paquetes = pack;
    }

    public void setCategorias(webServiceCategoria.categoriaData[] categoria) {
        categorias = categoria;
    }

    public void setMaterias(webServiceMateria.materiaData[] materia) {
        materias = materia;
    }
    public void setEjerciocio(webServiceEjercicio.ejercicioData[] ejercicio) {
        ejercicios = ejercicio;
    }
    public void setPreguntas(webServicePreguntas.preguntaData[] pregunta) {
        preguntas = pregunta;
    }

    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.Log("Error. Check internet connection!");
        } else {
            Debug.Log("Si hay conexion");
            StartCoroutine(webServicePaquetes.getPaquetes());
            StartCoroutine(webServiceCategoria.getCategorias());
            StartCoroutine(webServiceMateria.getMaterias());
            StartCoroutine(webServiceEjercicio.getEjercicios());
            StartCoroutine(webServicePreguntas.getPreguntas());
        }
    }

    public void Update() {
        validarPaquetes();
        validarCategorias();
        validarMaterias();
        validarEjercicios();
        validarPreguntas();
        //validarRespuestas();
    }

    public void validarPaquetes() {
        if (paquetes != null && banderaPaquetes) {
            foreach (var pack in paquetes) {
                if (webServicePaquetes.getPaquetesByDescripcionSqLite(pack.descripcion) != null) {
                    //Ya existe el paquete en local
                    Debug.Log("Ya existe el paquete en local");
                } else {
                    //Insertar paquete en local
                    webServicePaquetes.insertarPaqueteSqLite(pack.descripcion, pack.fechaRegistro, pack.fechaModificacion);
                }
            }
            banderaPaquetes = false;
        }
    }

    public void validarCategorias() {
        if (categorias != null && banderaCategorias) {
            foreach (var categoria in categorias) {
                if (webServiceCategoria.getCategoriaByDescripcionSqLite(categoria.descripcion) != null) {
                    //Ya existe el paquete en local
                    Debug.Log("Ya existe la categoria en local");
                } else {
                    //Insertar paquete en local
                    webServiceCategoria.insertarCategoriaSqLite(categoria.descripcion, categoria.status, categoria.fechaRegistro, categoria.fechaModificacion);
                }
            }
            banderaCategorias = false;
        }
    }

    public void validarMaterias() {
        if (materias != null && banderaMaterias) {
            foreach (var materia in materias) {
                if (webServiceMateria.getMateriaByClaveSqLite(materia.claveMateria) != null) {
                    //Ya existe el paquete en local
                    Debug.Log("Ya existe la materia en local");
                } else {
                    //Insertar paquete en local
                    string idCategoria = webServiceCategoria.getIdCategoriaByNameSqLite(materia.descripcionCategoria);
                    webServiceMateria.insertarMateriaSqLite(materia.claveMateria, materia.descripcion, materia.status, materia.fechaRegistro, materia.fechaModificacion, idCategoria);
                }
            }
            banderaMaterias = false;
        }
    }

    public void validarEjercicios() {
        if (materias != null && banderaEjercicios) {
            foreach (var ejercicio in ejercicios) {
                if (webServiceEjercicio.getEjercicioByDescripcionSqLite(ejercicio.descripcion) != null) {
                    //Ya existe el paquete en local
                    Debug.Log("Ya existe el ejercicio en local");
                } else {
                    //Insertar paquete en local
                    webServiceEjercicio.insertarEjercicioSqLite(ejercicio.descripcion, ejercicio.status, ejercicio.fechaRegistro, ejercicio.fechaModificacion);
                }
            }
            banderaEjercicios = false;
        }
    }

    public void validarPreguntas() {
        if (preguntas != null && banderaPreguntas) {
            foreach (var pregunta in preguntas) {
                if (webServicePreguntas.getPreguntaByDescripcionSqLite(pregunta.descripcion) != null) {
                    //Ya existe el paquete en local
                    Debug.Log("Ya existe la pregunta en local");
                } else {
                    //Insertar paquete en local
                    string idTipoEjercicio = webServiceEjercicio.getEjercicioByDescripcionSqLite(pregunta.descripcionEjercicio).id;
                    string idMateria = webServiceMateria.getMateriaByClaveSqLite(pregunta.claveMateria).id;
                    string idPaquete = webServicePaquetes.getPaquetesByDescripcionSqLite(pregunta.descripcionPaquete).id;
                    Debug.Log(pregunta.descripcion + pregunta.status + pregunta.fechaRegistro + pregunta.fechaModificacion + idTipoEjercicio + idMateria + idPaquete);
                    webServicePreguntas.insertarPreguntaSqLite(pregunta.descripcion, pregunta.status, pregunta.fechaRegistro, pregunta.fechaModificacion, idTipoEjercicio, idMateria, idPaquete);
                }
            }
            banderaPreguntas = false;
        }
    }

    void OnApplicationQuit() {
        if (Usuario != "") {
            webServiceRegistro.insertarRegistroSqLite("LogOut", Usuario, 3);
            webServiceLog.cerrarLog(Usuario);
        }
    }
    
}
