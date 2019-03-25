﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;


public class appManager : MonoBehaviour {

    private string Usuario = "";            ///< Usuario almacena el usuario que utiliza la aplicación
    private string idUsuario = "";          ///< idUsuario almacena el id del usuario que utiliza la aplicación
    private string Nombre = "";             ///< Nombre almacena el nombre del usuario que utiliza la aplicación
    private string Correo = "";             ///< Correo almacena el correo con el cual la cuenta esta vinculada
    private string Imagen = "";             ///< Imagen almacena la imagen, ya sea de facebook o bien de UVEG de la persona que utiliza la aplicación
    private webServicePaquetes.paqueteData[] paquetes = null;       ///< paquetes arreglo de estructura paqueteData, almacena los paquetes que existen en la BD del SII
    private bool banderaPaquetes = true;                            ///< banderaPaquetes verifica si ya se recorrio el arreglo paquetes
    private webServiceCategoria.categoriaData[] categorias = null;  ///< categorias arreglo de estructura categoriasData, almacena las categorias que existen en la BD del SII
    private bool banderaCategorias = true;                          ///< banderaCategorias verifica si ya se recorrio el arreglo categorias
    private webServiceAcciones.accionData[] acciones = null;        ///< acciones arreglo de estructura accionesData, almacena las acciones que existen en la BD del SII
    private bool banderaAcciones = true;                            ///< banderaAcciones verifica si ya se recorrio el arreglo acciones
    private webServiceEjercicio.ejercicioData[] ejercicios = null;  ///< ejercicios arreglo de estructura ejerciciosData, almacena las ejercicios que existen en la BD del SII
    private bool banderaEjercicios = true;                          ///< banderaEjercicios verifica si ya se recorrio el arreglo ejercicios
    private webServicePreguntas.preguntaData[] preguntas = null;    ///< preguntas arreglo de estructura preguntasData, almacena las preguntas que existen en la BD del SII
    private bool banderaPreguntas = true;                           ///< banderaPreguntas verifica si ya se recorrio el arreglo preguntas
    private webServiceRespuestas.respuestaData[] respuestas = null; ///< respuestas arreglo de estructura respuestasData, almacena las respuestas que existen en la BD del SII
    private bool banderaRespuestas = true;                          ///< banderaRespuestas verifica si ya se recorrio el arreglo respuestas
    private bool bandera = true;                                    ///< bandera verifica si ya se mostro la imagen de usuario

    public webServicePreguntas.preguntaData[] preguntasCategoria = null;    ///< preguntasCategoria arreglo de preguntas correspondientes al paquete que se selecciono para jugar
    public float numeroPreguntas = 5;                       ///< numeroPreguntas numero de preguntas que tendra el curso, el usuario puede modificarlo inGame
    public bool isOnline = false;                           ///< isOnline bandera que sirve para validar si se encuentra conectado a internet o no
    public webServicePaquetes.paqueteData packToPlay;       ///< packToPlay estructura del paquete que se va a jugar, descargar o actualizar
    public bool isFirstLogin = true;

    public string lastIdLog = "0";
    public float sizeCamera = 60;

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
        banderaAcciones = valor;
        banderaPaquetes = valor;
        banderaPreguntas = valor;
        banderaRespuestas = valor;
    }
    /**
     * Regresa los datos del usuario correspondiente al usuario
     * 
     */
    public string getIdUsuario() {
        return idUsuario;
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

    public void setAcciones(webServiceAcciones.accionData[] Acciones) {
        acciones = Acciones;
    }

    public webServiceAcciones.accionData[] getAcciones() {
        return acciones;
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

    /**
     * Funcion que se llama antes de mostrar video (frame 0)
     * Verifica si hay conexion a internet o no
     */
    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            isOnline = false;
        } else {
            isOnline = true;
        }
    }

    /**
     * Conjunto de metodos y variables que sirven para mostrar una consola inGame (debugs en oculus y android)
     * 
     */
    #region funciones para consola in game
    string myLog;
    Queue myLogQueue = new Queue();
    /*void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        myLog = logString;
        string newString = "";
        if (type == LogType.Error || type == LogType.Exception) {
            newString += "\n*****************";
        }
        newString += "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception) {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue) {
            myLog += mylog;
        }
    }*/
    #endregion

    /**
     * Funcion que se manda llamar cada frame
     * verifica si existe la imagen del usuario, en caso que no exista la consulta e inserta
     * se encarga de llamar las validaciones de los datos de la BD
     */
    public void Update() {
        if (GameObject.Find("LeftEyeAnchor").GetComponent<Camera>().fieldOfView != sizeCamera) {
            GameObject.Find("LeftEyeAnchor").GetComponent<Camera>().fieldOfView = sizeCamera;
        }
        if (GameObject.Find("RightEyeAnchor").GetComponent<Camera>().fieldOfView != sizeCamera) {
            GameObject.Find("RightEyeAnchor").GetComponent<Camera>().fieldOfView = sizeCamera;
        }
        GameObject.Find("Player").GetComponent<PlayerManager>().setMensaje2(true, myLog);
        if (isOnline) {
            if (Usuario != "" && bandera) {
                if (Imagen == "") {
                    StartCoroutine(webServiceUsuario.getUserData(Usuario));
                    bandera = false;
                }
            }
        } else {
            setUsuario("Invitado");
            setNombre("Invitado");
            setCorreo("");
            setImagen("http://sii.uveg.edu.mx/unity/dropsV2/img/invitado.png");
        }
        validarCategorias();
        validarPaquetes();
        validarAcciones();
        validarEjercicios();
        validarPreguntas();
        validarRespuestas();
    }

    /**
     * Funcion que se encarga de validar si ya se descargo o no cada uno de los paquetes
     * que se encuentran en la BD.
     */
    public void validarPaquetes() {
        if (GameObject.Find("ListaPaquetes")) {
            var paquetesManager = GameObject.Find("ListaPaquetes").GetComponent<paquetesManager>();
            if (paquetes != null && banderaPaquetes) {
                paquetesManager.destruirObjetos(null);
                destruirObjetos();
                foreach (var pack in paquetes) {
                    var local = webServicePaquetes.getPaquetesByDescripcionSqLite(pack.descripcion);
                    if (local != null) {
                        pack.id = local.id;
                        var descargaLocal = webServiceDescarga.getDescargaByPaquete(pack.id);
                        if (descargaLocal == null) {
                            if (isOnline) {
                                paquetesManager.newCardDescarga(pack);
                            } else {

                            }
                        } else {
                            if (isOnline) {
                                if (isActualized(descargaLocal.fechaDescarga, pack.fechaModificacion)) {
                                    paquetesManager.newCardJugar(pack, null);
                                } else {
                                    paquetesManager.newCardActualizar(pack, null);
                                }
                            } else {
                                paquetesManager.newCardJugar(pack, null);
                            }
                        }
                    } else {
                        webServicePaquetes.insertarPaqueteSqLite(pack);
                        paquetesManager.newCardDescarga(pack);
                    }
                }
                paquetesManager.fillEmpty(null);
                banderaPaquetes = false;
            }
        }
    }

    /**
     * Funcion que se encarga de validar si ya se descargo o no cada una de las categorias
     * que se encuentran en la BD.
     */
    public void validarCategorias() {
        if (categorias != null && banderaCategorias) {
            foreach (var categoria in categorias) {
                var local = webServiceCategoria.getCategoriaByDescripcionSqLite(categoria.descripcion);
                if (local != null) {
                    categoria.id = local.id;
                } else {
                    webServiceCategoria.insertarCategoriaSqLite(categoria);
                }
            }
            banderaCategorias = false;
        }
    }

    /**
      * Funcion que se encarga de validar si ya se descargo o no cada una de las acciones
      * que se encuentran en la BD.
      */
    public void validarAcciones() {
        if (acciones != null && banderaAcciones) {
            foreach (var Acciones in acciones) {
                var local = webServiceAcciones.consultarIdAccionSqLite(Acciones.descripcion);
                if (local != "0") {
                    Acciones.id = local;
                } else {
                    webServiceAcciones.insertarAccionSqLite(Acciones.descripcion, Acciones.status);
                }
            }
            banderaAcciones = false;
        }
    }

    /**
     * Funcion que se encarga de validar si ya se descargo o no cada uno de los ejercicios
     * que se encuentran en la BD.
     */
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

    /**
     * Esta funcion solo se manda llamar cada que el usuario da click en descargar o actualizar
     * Verifica cada una de las preguntas de determinado paquete para saber si ya se realizó la descarga
     * o verificar si es necesario actualizar los datos
     */
    public void validarPreguntas() {
        if (preguntas != null && banderaPreguntas) {
            banderaPreguntas = false;
            foreach (var pregunta in preguntas) {
                var local = webServicePreguntas.getPreguntaByIdServerSqLite(pregunta.id);
                if (local != null) {
                    pregunta.id = local.id;
                    pregunta.idPaquete = local.idPaquete;
                    pregunta.idTipoEjercicio = local.idTipoEjercicio;
                    webServicePreguntas.updatePreguntaSqLite(pregunta, local.idServer);
                } else {
                    string idTipoEjercicio = webServiceEjercicio.getEjercicioByDescripcionSqLite(pregunta.descripcionEjercicio).id;
                    //Error Aqui
                    string idPaquete = webServicePaquetes.getPaquetesByDescripcionSqLite(pregunta.descripcionPaquete).id;
                    webServicePreguntas.insertarPreguntaSqLite(pregunta.descripcion, pregunta.status, pregunta.fechaRegistro, pregunta.fechaModificacion, idTipoEjercicio, idPaquete, pregunta.id);
                }
            }
        }
    }

    /**
     * Esta funcion solo se manda llamar cada que el usuario da click en descargar o actualizar
     * Verifica cada una de las respuestas de determinada pregunta para saber si ya se realizó la descarga
     * o verificar si es necesario actualizar los datos
     */
    public void validarRespuestas() {
        if (respuestas != null && banderaRespuestas) {
            banderaRespuestas = false;
            foreach (var respuesta in respuestas) {
                ////Error aqui
                if (webServicePreguntas.getPreguntaByIdServerSqLite(respuesta.idPregunta) != null) {
                    var idPregunta = webServicePreguntas.getPreguntaByIdServerSqLite(respuesta.idPregunta).id;
                    var local = webServiceRespuestas.getRespuestaByIdServerAndPreguntaSquLite(respuesta.id, idPregunta);
                    if (local != null) {
                        webServiceRespuestas.updateRespuestaSqLite(respuesta.descripcion, respuesta.urlImagen, respuesta.correcto, respuesta.relacion, respuesta.status, respuesta.fechaRegistro, respuesta.fechaModificacion, idPregunta, local.idServer);
                        respuesta.id = local.id;
                        respuesta.idPregunta = local.idPregunta;
                    } else {
                        webServiceRespuestas.insertarRespuestaSqLite(respuesta.descripcion, respuesta.urlImagen, respuesta.correcto, respuesta.relacion, respuesta.status, respuesta.fechaRegistro, respuesta.fechaModificacion, idPregunta, respuesta.id);
                    }
                }
            }
            StartCoroutine(descargarImagenesPaquete());
        }
    }

    /**
     * Coorutina que se encarga de descargar las imagenes de el paquete que se esta descargando
     * o actualizando, verifica si la fecha de modificacion de la respuesta es diferente a la de 
     * descarga anterior y comienza la descarga.
     * Aqui se oculta el mensaje "Descargando paquete"
     * y se encarga de actulizar los paquetes en pantalla
     */
    public IEnumerator descargarImagenesPaquete() {
        foreach (var respuesta in respuestas) {
            var descarga = webServiceDescarga.getDescargaByPaquete(packToPlay.id);
            //Validar si fecha modificacion respuesta es diferente a la fecha de descarga que se tenia
            if (descarga != null) {
                if (respuesta != null) {
                    if (!isActualized(descarga.fechaDescarga, respuesta.fechaModificacion)) {
                        //Debug.Log(respuesta.urlImagen);
                        //if (respuesta.urlImagen != "") {
                            var pathArray = respuesta.urlImagen.Split('/');
                            var path = pathArray[pathArray.Length - 1];
                            WWW www = new WWW(respuesta.urlImagen);
                            yield return www;
                            if (www.texture != null) {
                                Texture2D texture = www.texture;
                                byte[] bytes = texture.EncodeToPNG();
                                File.WriteAllBytes(Application.persistentDataPath + path, bytes);
                            } else {

                            }
                        //}
                    } else {
                    }
                } else {

                }
            } else {
                //Debug.Log(respuesta.urlImagen);
                //if (respuesta.urlImagen != "") {
                    var pathArray = respuesta.urlImagen.Split('/');
                    var path = pathArray[pathArray.Length - 1];
                    WWW www = new WWW(respuesta.urlImagen);
                    yield return www;
                    if (www.texture != null) {
                        Texture2D texture = www.texture;
                        byte[] bytes = texture.EncodeToPNG();
                        File.WriteAllBytes(Application.persistentDataPath + path, bytes);
                    } else {

                    }
                //}
            }
        }
        webServiceDescarga.insertarDescargaSqLite(webServicePaquetes.getPaquetesByDescripcionSqLite(packToPlay.descripcion).id, webServiceUsuario.consultarIdUsuarioSqLite(Usuario));
        GameObject.Find("Player").GetComponent<PlayerManager>().setMensaje(true, "Paquete descargado");
        GameObject.Find("Player").GetComponent<PlayerManager>().setMensaje(false, "");

        Destroy(GameObject.Find("fichaPack" + preguntas[0].idPaquete));
        banderaPaquetes = true;
        validarPaquetes();
        preguntas = null;
        banderaPreguntas = true;
        respuestas = null;
        banderaRespuestas = true;
    }

    /**
     * Funcion que se manda llamar al momento de cerrar la aplicacion
     * Modifica la fecha de termino del log en la base de datos local
     */
    void OnApplicationQuit() {
        if (Usuario != "") {
            webServiceRegistro.validarAccionSqlite("Aplicación cerrada por el usuario", Usuario, "Aplicación cerrada");
            webServiceLog.cerrarLog(Usuario);
        }
    }

    /**
     * Funcion que verifica si el paquete esta actualizado o no
     * Verifica si la fecha de descarga que se tiene es menor, mayor o igual a la fecha de modificacion del paquete
     * @descargalocal descargaData estructura que almacena los datos de la descarga anterior
     * @pack paqueteData estructura que almacena los datos del paquete que viene desde BD del SII
     */
    public bool isActualized(string fechaDescarga, string fechaModificacion) {
        //Formato de fechaDescarga = dd/MM/yyyy HH:mm:ss "PC"
        //Formato de fechaDescarga = MM/dd/yyyy HH:mm:ss "Android"
        var dia = 1;
        var mes = 0;
        var año = 2;
        if (Application.isEditor) {
            dia = 0;
            mes = 1;
            año = 2;
        }
        fechaDescarga = fechaDescarga.Remove(10, fechaDescarga.Length - 10);
        string[] splitDateDescarga = fechaDescarga.Split('/');
        //Formato de fechaModificacion paquete = yyyy-MM-dd HH:mm:ss
        fechaModificacion = fechaModificacion.Remove(10, fechaModificacion.Length - 10);
        string[] splitDatePack = fechaModificacion.Split('-');
        if (Int32.Parse(splitDateDescarga[año]) >= Int32.Parse(splitDatePack[0])) {
            if (Int32.Parse(splitDateDescarga[mes]) >= Int32.Parse(splitDatePack[1])) {
                if (Int32.Parse(splitDateDescarga[mes]) == Int32.Parse(splitDatePack[1])) {
                    if (Int32.Parse(splitDateDescarga[dia]) >= Int32.Parse(splitDatePack[2])) {
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

    /**
     * Funcion que se manda llamar cada que se actualiza/descarga un paquete
     * Limpia los campos dentro de panelPaquetesDescargados y PanelNuevosPaquetes
     * para despues llenarlos de nuevo con la informacion actualizada
     */
    void destruirObjetos() {
        if (GameObject.Find("PanelNuevosPaquetes").transform.childCount > 0) {
            for (var i = 0; i < GameObject.Find("PanelNuevosPaquetes").transform.childCount; i++) {
                var objeto = GameObject.Find("PanelNuevosPaquetes").transform.GetChild(i);
                DestroyImmediate(objeto.gameObject);
            }
            destruirObjetos();
        } else {
            return;
        }
    }

}
