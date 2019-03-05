﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;


public class appManager : MonoBehaviour {

    private string Usuario = "";            ///< Usuario almacena el usuario que utiliza la aplicación
    private string idUsuario = "";
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
    private webServiceRespuestas.respuestaData[] respuestas = null;
    private bool banderaRespuestas = true;
    private bool bandera = true;

    public webServicePreguntas.preguntaData[] preguntasCategoria = null;

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
    public void setBanderas(bool valor) {
        banderaCategorias = valor;
        banderaEjercicios = valor;
        banderaMaterias = valor;
        banderaPaquetes = valor;
        banderaPreguntas = valor;
        banderaRespuestas = valor;
    }
    /**
     * Regresa los datos del usuario correspondiente al usuario
     * 
     */
    public string getUsuario() {
        return Usuario;
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

    public void setPaquetes(webServicePaquetes.paqueteData[] pack) {
        paquetes = pack;
    }

    public webServicePaquetes.paqueteData[] GetPaquetes() {
        return paquetes;
    }

    public void setCategorias(webServiceCategoria.categoriaData[] categoria) {
        categorias = categoria;
    }

    public webServiceCategoria.categoriaData[] getCategorias() {
        return categorias;
    }

    public void setMaterias(webServiceMateria.materiaData[] materia) {
        materias = materia;
    }

    public webServiceMateria.materiaData[] getMaterias() {
        return materias;
    }

    public void setEjerciocio(webServiceEjercicio.ejercicioData[] ejercicio) {
        ejercicios = ejercicio;
    }

    public webServiceEjercicio.ejercicioData[] getEjecicios() {
        return ejercicios;
    }

    public void setPreguntas(webServicePreguntas.preguntaData[] pregunta) {
        preguntas = pregunta;
    }

    public webServicePreguntas.preguntaData[] getPreguntas() {
        return preguntas;
    }

    public void setRespuestas(webServiceRespuestas.respuestaData[] respuesta) {
        respuestas = respuesta;
    }

    public webServiceRespuestas.respuestaData[] getRespuestas() {
        return respuestas;
    }
    #endregion

    public void Awake() {
        GameObject.Find("MensajeCarga").SetActive(false);
        DontDestroyOnLoad(this.gameObject);
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.Log("No hay conexion");
        } else {
            Debug.Log("Si hay conexion");
        }
    }

    public void Update() {
        if (Usuario != "" && bandera) {
            if (Imagen == "") {
                StartCoroutine(webServiceUsuario.getUserData(Usuario));
                bandera = false;
            }
        }
        validarPaquetes();
        validarCategorias();
        validarMaterias();
        validarEjercicios();
        validarPreguntas();
        validarRespuestas();
    }

    public void validarPaquetes() {
        if (GameObject.Find("ListaPaquetes")) {
            var paquetesManager = GameObject.Find("ListaPaquetes").GetComponent<paquetesManager>();
            if (paquetes != null && banderaPaquetes) {
                destruirObjetos();
                foreach (var pack in paquetes) {
                    var local = webServicePaquetes.getPaquetesByDescripcionSqLite(pack.descripcion);
                    if (local != null) {
                        pack.id = local.id;
                        var descargaLocal = webServiceDescarga.getDescargaByPaquete(pack.id);
                        //Si no existe la descarga del paquete añade la tarjeta
                        if (descargaLocal == null) {
                            paquetesManager.newCardDescarga(pack);
                        } else {
                            if (isActualized(descargaLocal, pack)) {
                                //Esta actualizado
                                paquetesManager.newCardJugar(pack);
                            } else {
                                //No esta actualizado
                                paquetesManager.newCardActualizar(pack);
                            }
                        }
                    } else {
                        webServicePaquetes.insertarPaqueteSqLite(pack.descripcion, pack.fechaRegistro, pack.fechaModificacion, pack.urlImagen, pack.id);
                        paquetesManager.newCardDescarga(pack);
                    }
                }
                banderaPaquetes = false;
            }
        }
    }

    

    public void validarCategorias() {
        if (categorias != null && banderaCategorias) {
            foreach (var categoria in categorias) {
                var local = webServiceCategoria.getCategoriaByDescripcionSqLite(categoria.descripcion);
                if (local != null) {
                    categoria.id = local.id;
                } else {
                    webServiceCategoria.insertarCategoriaSqLite(categoria.descripcion, categoria.status, categoria.fechaRegistro, categoria.fechaModificacion);
                }
            }
            banderaCategorias = false;
        }
    }

    public void validarMaterias() {
        if (materias != null && banderaMaterias) {
            foreach (var materia in materias) {
                var local = webServiceMateria.getMateriaByClaveSqLite(materia.claveMateria);
                if (local != null) {
                    materia.id = local.id;
                    materia.idCategoria = local.idCategoria;
                } else {
                    string idCategoria = webServiceCategoria.getIdCategoriaByNameSqLite(materia.descripcionCategoria);
                    webServiceMateria.insertarMateriaSqLite(materia.claveMateria, materia.descripcion, materia.status, materia.fechaRegistro, materia.fechaModificacion, idCategoria);
                }
            }
            banderaMaterias = false;
        }
    }

    public void validarEjercicios() {
        if (ejercicios != null && banderaEjercicios) {
            foreach (var ejercicio in ejercicios) {
                var local = webServiceEjercicio.getEjercicioByDescripcionSqLite(ejercicio.descripcion);
                if (local != null) {
                    ejercicio.id = local.id;
                } else {
                    webServiceEjercicio.insertarEjercicioSqLite(ejercicio.descripcion, ejercicio.status, ejercicio.fechaRegistro, ejercicio.fechaModificacion);
                }
            }
            banderaEjercicios = false;
        }
    }

    public void validarPreguntas() {
        if (preguntas != null && banderaPreguntas) {
            Debug.Log("Validando preguntas");
            banderaPreguntas = false;
            foreach (var pregunta in preguntas) {
                var local = webServicePreguntas.getPreguntaByDescripcionSqLite(pregunta.descripcion);
                if (local != null) {
                    pregunta.id = local.id;
                    pregunta.idMateria = local.idMateria;
                    pregunta.idPaquete = local.idPaquete;
                    pregunta.idTipoEjercicio = local.idTipoEjercicio;
                } else {
                    string idTipoEjercicio = webServiceEjercicio.getEjercicioByDescripcionSqLite(pregunta.descripcionEjercicio).id;
                    string idMateria = webServiceMateria.getMateriaByClaveSqLite(pregunta.claveMateria).id;
                    string idPaquete = webServicePaquetes.getPaquetesByDescripcionSqLite(pregunta.descripcionPaquete).id;
                    webServicePreguntas.insertarPreguntaSqLite(pregunta.descripcion, pregunta.status, pregunta.fechaRegistro, pregunta.fechaModificacion, idTipoEjercicio, idMateria, idPaquete, pregunta.id);
                }
            }
        }
    }

    public void validarRespuestas() {
        if (respuestas != null && banderaRespuestas) {
            Debug.Log("Validando respuestas");
            banderaRespuestas = false;
            foreach (var respuesta in respuestas) {
                var idPregunta = webServicePreguntas.getPreguntaByDescripcionSqLite(respuesta.descripcionPregunta);
                var local = webServiceRespuestas.getRespuestaByDescripcionAndPreguntaSquLite(respuesta.descripcion, idPregunta.id);
                if (local != null) {
                    Debug.Log("Ya existe esta pregunta");
                    webServiceRespuestas.updateRespuestaSqLite(respuesta.descripcion,respuesta.urlImagen, respuesta.correcto, respuesta.relacion, respuesta.status, respuesta.fechaRegistro, respuesta.fechaModificacion, respuesta.idPregunta, respuesta.idServer);
                    respuesta.id = local.id;
                    respuesta.idPregunta = local.idPregunta;
                } else {
                    webServiceRespuestas.insertarRespuestaSqLite(respuesta.descripcion, respuesta.urlImagen, respuesta.correcto, respuesta.relacion, respuesta.status, respuesta.fechaRegistro, respuesta.fechaModificacion, idPregunta.id, respuesta.id);
                }
            }
            StartCoroutine(descargarImagenesPaquete());
        }
    }

    public IEnumerator descargarImagenesPaquete() {
        foreach (var respuesta in respuestas) {
            var pathArray = respuesta.urlImagen.Split('/');
            var path = pathArray[pathArray.Length - 1];
            if (File.Exists(Application.persistentDataPath + path)) {
                byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            } else {
                WWW www = new WWW(respuesta.urlImagen);
                yield return www;
                if (www.texture != null) {
                    Texture2D texture = www.texture;
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath + path, bytes);
                } else {
                    Debug.Log("Texture es null: " + respuesta.urlImagen);
                }
            }
        }
        var mensaje = GameObject.Find("MensajeCarga");
        if (mensaje) {
            mensaje.GetComponentInChildren<Text>().text = "Paquete descargado";
            mensaje.SetActive(false);
        }
        Destroy(GameObject.Find("fichaPack" + preguntas[0].idPaquete));
        banderaPaquetes = true;
        validarPaquetes();
        preguntas = null;
        banderaPreguntas = true;
        respuestas = null;
        banderaRespuestas = true;
    }

    void OnApplicationQuit() {
        if (Usuario != "") {
            webServiceRegistro.insertarRegistroSqLite("LogOut", Usuario, 3);
            webServiceLog.cerrarLog(Usuario);
        }
    }

    bool isActualized(webServiceDescarga.descargaData descargaLocal, webServicePaquetes.paqueteData pack) {
        //Formato de fechaDescarga = dd/MM/yyyy HH:mm:ss
        descargaLocal.fechaDescarga = descargaLocal.fechaDescarga.Remove(10, descargaLocal.fechaDescarga.Length - 10);
        string[] splitDateDescarga = descargaLocal.fechaDescarga.Split('/');
        //Formato de fechaModificacion paquete = yyyy-MM-dd HH:mm:ss
        pack.fechaModificacion = pack.fechaModificacion.Remove(10, pack.fechaModificacion.Length - 10);
        string[] splitDatePack = pack.fechaModificacion.Split('-');
        if (Int32.Parse(splitDateDescarga[2]) >= Int32.Parse(splitDatePack[0])) {
            if (Int32.Parse(splitDateDescarga[1]) >= Int32.Parse(splitDatePack[1])) {
                if (Int32.Parse(splitDateDescarga[1]) == Int32.Parse(splitDatePack[1])) {
                    if (Int32.Parse(splitDateDescarga[0]) >= Int32.Parse(splitDatePack[2])) {
                        return true;
                    } else {
                        return false;
                    }
                }
            } else {
                return false;
            }
        } else {
            return false;
        }
        return true;
    }

    void destruirObjetos() {
        if (GameObject.Find("PanelPaquetesDescargados").transform.childCount > 0) {
            for (var i = 0; i < GameObject.Find("PanelPaquetesDescargados").transform.childCount; i++) {
                var objeto = GameObject.Find("PanelPaquetesDescargados").transform.GetChild(i);
                Destroy(objeto.gameObject);
            }
        }
        if (GameObject.Find("PanelNuevosPaquetes").transform.childCount > 0) {
            for (var i = 0; i < GameObject.Find("PanelNuevosPaquetes").transform.childCount; i++) {
                var objeto = GameObject.Find("PanelNuevosPaquetes").transform.GetChild(i);
                Destroy(objeto.gameObject);
            }
        }
    }

}
