using System.Collections;
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
        if (GameObject.Find("fichasPaquetes")) {
            var listaPacks = GameObject.Find("fichasPaquetes");
            if (paquetes != null && banderaPaquetes) {
                foreach (var pack in paquetes) {
                    Debug.Log(pack.descripcion);
                    var local = webServicePaquetes.getPaquetesByDescripcionSqLite(pack.descripcion);
                    if (local != null) {
                        pack.id = local.id;
                        var descargaLocal = webServiceDescarga.getDescargaByPaquete(pack.id);
                        if (descargaLocal == null) {
                            var fichaPaquete = Instantiate(Resources.Load("fichaPaquete") as GameObject);
                            fichaPaquete.name = "fichaPack" + pack.id;
                            llenarFicha(fichaPaquete, pack.descripcion, pack.fechaModificacion);
                            fichaPaquete.transform.SetParent(listaPacks.transform);
                            fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                            fichaPaquete.GetComponent<packManager>().paquete = pack.descripcion;
                            fichaPaquete.GetComponent<packManager>().paqueteId = pack.id;
                        } else {

                        }
                    } else {
                        var fichaPaquete = Instantiate(Resources.Load("fichaPaquete") as GameObject);
                        fichaPaquete.name = "fichaPack" + pack.id;
                        llenarFicha(fichaPaquete, pack.descripcion, pack.fechaModificacion);
                        fichaPaquete.transform.SetParent(listaPacks.transform);
                        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                        webServicePaquetes.insertarPaqueteSqLite(pack.descripcion, pack.fechaRegistro, pack.fechaModificacion);
                        fichaPaquete.GetComponent<packManager>().paquete = pack.descripcion;
                        fichaPaquete.GetComponent<packManager>().paqueteId = pack.id;
                    }
                }
                banderaPaquetes = false;
            }
            if (listaPacks.transform.childCount <= 0) {
                GameObject.Find("ListaPaquetes").GetComponent<testMaterias>().textoPaquetes.SetActive(true);
            } else {
                GameObject.Find("ListaPaquetes").GetComponent<testMaterias>().textoPaquetes.SetActive(false);
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
            banderaPreguntas = false;
            Debug.Log("Hay preguntas");
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
                    webServicePreguntas.insertarPreguntaSqLite(pregunta.descripcion, pregunta.status, pregunta.fechaRegistro, pregunta.fechaModificacion, idTipoEjercicio, idMateria, idPaquete);
                }
            }
        }
    }

    public void validarRespuestas() {
        if (respuestas != null && banderaRespuestas) {
            banderaRespuestas = false;
            Debug.Log(respuestas.Length);
            foreach (var respuesta in respuestas) {
                var idPregunta = webServicePreguntas.getPreguntaByDescripcionSqLite(respuesta.descripcionPregunta);
                Debug.Log(idPregunta.id);
                var local = webServiceRespuestas.getRespuestaByDescripcionAndPreguntaSquLite(respuesta.descripcion, idPregunta.id);
                if (local != null) {
                    respuesta.id = local.id;
                    respuesta.idPregunta = local.idPregunta;
                } else {
                    webServiceRespuestas.insertarRespuestaSqLite(respuesta.descripcion, respuesta.urlImagen, respuesta.correcto, respuesta.relacion, respuesta.status, respuesta.fechaRegistro, respuesta.fechaModificacion, idPregunta.id);
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
                Texture2D texture = www.texture;
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(Application.persistentDataPath + path, bytes);
            }
        }
        Destroy(GameObject.Find("fichaPack" + preguntas[0].idPaquete));
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

    void llenarFicha(GameObject ficha, string descripcion, string fechaModificacion) {
        ficha.transform.GetChild(0).GetComponent<Text>().text = descripcion;
        var fecha = fechaModificacion.Split(' ');
        ficha.transform.GetChild(1).GetComponent<Text>().text = "Ultima actualización:\n" + fecha[0];
    }

}
