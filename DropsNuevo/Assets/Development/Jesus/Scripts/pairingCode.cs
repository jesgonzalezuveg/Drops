using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;
using UnityEngine.Events;

public class pairingCode : MonoBehaviour
{
    //Variables que guardan el codigo generado, la segunda con guión en medio de los 6 caracteres
    string code;
    string code2;

    public static string status;
    public static int valCodigoSii;
    public static string idCodigoServer;
    private int salir;
    private int countFrames;
    private int cargaCodigo;
    UnityEvent listenerCode= new UnityEvent();

    //Variables de emparejamiento con sqlite
    public static string fechaInicio;
    public static string fechaTermino;
    public static string dispositivo;
    public static string idCodigo;
    public static string idUsuario;
    public static string usuario;
    public static string nombre;
    public static string rol;
    public static string gradoEstudios;
    public static string programa;
    public static string fechaRegistro;
    public static string statusUsuario;
    public static string idC;
    public static string codigo;

    [Serializable]
    public class DataUser {
        public string id = "";
        public string nombre = "";
        public string rol = "";
        public string gradoEstudios = "";
        public string programa = "";
        public string fechaRegistro = "";
        public string status = "";
        public string syncroStatus = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        code = "";
        code2 = "";
        status = "5";
        valCodigoSii = 3;
        countFrames = 0;
        cargaCodigo = 0;
        salir = 0;
        code = "aLRB52";
    }

    // Update is called once per frame
    void Update()
    {
        if (countFrames>=30) {

            //Paso 1 de pairing code: generara el codigo y guardarlo en el servidor
            if (salir == 0) {
                StartCoroutine(WebServiceCodigo.obtenerCodigo(code, 1));
                if (valCodigoSii == 1) {
                    pairingCode.valCodigoSii = 3;
                    Debug.Log("El código ya exixte");
                    code = generateCode();
                } else if (valCodigoSii == 0) {
                    pairingCode.valCodigoSii = 3;
                    Debug.Log("El código no exixte");
                    guardarCodigoSqlite(code);
                    StartCoroutine(WebServiceCodigo.insertarCodigo(code));
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = code2;
                    listenerCode.AddListener(emparejarCodigo);
                    salir = 1;
                } else {
                    Debug.Log("Esperando Respuesta del Web Service 1");
                }
            }

            //Paso 2 de pairing code: verificar si el codio es tomado por algun usuario para emparejarlo y generar la sesión
            // Iniciar Listener
            if (salir == 1) {
                if (status == "2") {
                    Debug.Log("Quitting");
                    int res = editarCodigoSqlite(code, 2);
                    if (res == 1) {
                        Debug.Log("Se modifico el status");
                        listenerCode.RemoveListener(emparejarCodigo);
                        salir = 2;
                        GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "Codigo emparejado";
                        Debug.Log("Emparejando datos de sesion generados");
                        salir = 3;
                        pairingCode.valCodigoSii = 3;
                    } else {
                        Debug.Log("No se modifico el status");
                    }
                } else {
                    listenerCode.Invoke();
                    if (valCodigoSii == 1) {
                        pairingCode.valCodigoSii = 3;
                    } else if (valCodigoSii == 0) {
                        pairingCode.valCodigoSii = 3;
                    } else {
                        Debug.Log("Esperando Respuesta del Web Service 2");
                    }
                }
            }

            //Paso 3 de pairing code: sincronizar los datos de la sesion generada en el servidor con la db local
            if (salir == 3) {
                StartCoroutine(wsParingCode.getDataSesionByCode(idCodigoServer));
                if (valCodigoSii == 1) {
                    pairingCode.valCodigoSii = 3;
                    Debug.Log("Se obtuvieron los datos de sesion para emparejarlos");
                    //Validar si el usuario ya existe en la db local
                    var res = existUserSqlite(usuario);
                    if (res!=1) {
                        //Guardar el registro del usuario en la db local
                        var resSaveUser = saveUserSqlite(usuario, nombre, rol, gradoEstudios, programa, fechaRegistro, Int32.Parse(status));
                        if (resSaveUser == 1) {
                            Debug.Log("El usuario se guardo correctamente");
                            //Obtener datos del usuario que se acaba de registrar de la db local
                            var resultado = getDataUserSqlite(usuario);
                            if (resultado!="0") {
                                DataUser data = JsonUtility.FromJson<DataUser>(resultado);
                                //Guardar el log del usuario en la db local
                                Debug.Log("El usuario se guardo correctamente");
                                var resSaveLog = saveLogSqlite(fechaInicio, fechaTermino, dispositivo, 2, idCodigo, data.id);
                                if (resSaveLog == 1) {
                                    Debug.Log("El log se inserto correctamente");
                                    //Cambiar estado del codigo a 3 tanto en local
                                    var resEditCode = editarCodigoSqlite(codigo, 3);
                                    if (resEditCode == 1) {
                                        Debug.Log("El estado del codigo local se cambio correctammente");
                                        salir = 4;
                                    } else {
                                        Debug.Log("No se pudo realizar el combio del estado");
                                        salir = 5;
                                    }
                                } else {
                                    Debug.Log("El log no se inserto correctamente");
                                }
                            } else {
                                Debug.Log("El log no se inserto correctamente");
                            }
                        } else {
                            Debug.Log("No se encontro el usuario que se acaba de registrar");
                        }
                    } else {
                        Debug.Log("El usuario ya existe");
                        //Obtener datos del usuario ya registrado en la db local
                        var resultado = getDataUserSqlite(usuario);
                        if (resultado != "0") {
                            DataUser data = JsonUtility.FromJson<DataUser>(resultado);
                            //Guardar el log del usuario en la db local
                            var resSaveLogSqlite = saveLogSqlite(fechaInicio, fechaTermino, dispositivo, 2, idCodigo, data.id);
                            if (resSaveLogSqlite == 1) {
                                Debug.Log("El log se inserto correctamente");
                                //Cambiar estado del codigo a 3 tanto en local
                                var resEditSQLite = editarCodigoSqlite(codigo, 3);
                                if (resEditSQLite == 1) {
                                    Debug.Log("El estado del codigo local se cambio correctammente");
                                    salir = 4;
                                } else {
                                    Debug.Log("No se pudo realizar el combio del estado");
                                    salir = 5;
                                }
                            } else {
                                Debug.Log("El log no se inserto correctamente");
                            }
                        } else {
                            Debug.Log("No se encontro el usuario ya registrado");
                        }
                    }
                } else if (valCodigoSii == 0) {
                    //pairingCode.valCodigoSii = 3;
                    Debug.Log(valCodigoSii);
                    Debug.Log("No obtuvieron los datos de sesion para emparejarlos");
                } else {
                    Debug.Log("Esperando Respuesta del Web Service 3");
                }
            }

            //Paso 4 de pairing code: cambiar el estado del codigo del servidor a 3
            if (salir == 4) {
                StartCoroutine(WebServiceCodigo.updateCode(idCodigo, 3));
                if (valCodigoSii == 1) {
                    Debug.Log("Se actualizo codigo en servidor");
                    Debug.Log("Emparejamiento finalizado. Pasar a la siguiente escena");
                } else if (valCodigoSii == 0) {
                    Debug.Log("No se pudo actualizar codigo en servidor");
                } else {
                    Debug.Log("Esperando Respuesta del Web Service 4");
                }
            }

                countFrames = 0;
        } else {
            countFrames++;
            cargaCodigo++; if (salir==0) {
                if (cargaCodigo == 1) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "X";
                } else if (cargaCodigo == 2) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XD";
                } else if (cargaCodigo == 3) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDX";
                } else if (cargaCodigo == 4) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDXD";
                } else if (cargaCodigo == 5) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDXDX";
                } else if (cargaCodigo == 6) {
                    GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text = "XDXDXD";
                    cargaCodigo = 0;
                }
            }
        }
    }

    void emparejarCodigo() {
        //Output message to the console
        Debug.Log(status);
        Debug.Log(valCodigoSii);
        StartCoroutine(WebServiceCodigo.obtenerCodigo(code, 2));
    }

    /** Funcion que sirve para generar un código de 8 caracteres de manera aleatoria
   *
   *@param  chars Lista de caracteres 
   *@param  stringChars Contenedor de los 6 caracteres que contendra el codigo
   *@param  random Funcion para elección aleatoria
   *@param  finalString Código obtentido
   *@method conexionDB.alterGeneral metodo que recibe una query ya sea para realizar un insert, update o delete
   **/
    public string generateCode() {
        code2 = "";
        var chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[6];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++) {
            stringChars[i] = chars[random.Next(chars.Length)];
            if (i == 2) {
                code2 = code2 + stringChars[i] + "-";
            } else {
                code2 = code2 + stringChars[i];
            }
        }

        var finalString = new string(stringChars);

        return finalString;
    }

    private int guardarCodigoSqlite(string codigo) {
        string query = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('" + codigo + "', 1, datetime(), datetime())";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    private int editarCodigoSqlite(string codigo, int status) {
        string query = "UPDATE codigo SET status = "+ status + ", fechaModificacion = datetime() WHERE descripcion = '" + codigo + "' AND status = "+(status-1)+"";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    private int existeCodigoSqlite(string codigo) {
        string query = "SELECT * FROM codigo WHERE descripcion = '"+ codigo +"' AND status = 1";
        var result = conexionDB.selectGeneral(query);

        if (result != "0") {
            return 1;
        } else {
            return 0;
        }
    }


    private int saveUserSqlite(string usuario, string nombre, string rol, string gradoEstudios, string programa, string fechaRegistro, int status) {
        string query = "INSERT INTO usuario (usuario, nombre, rol, gradoEstudios, programa, fechaRegistro, status, syncroStatus) VALUES ('" + usuario + "', '" + nombre + "', '" + rol + "', '" + gradoEstudios + "', '" + programa + "', '" + fechaRegistro + "', "+ status + ", 2)";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    private int existUserSqlite(string usuario) {
        string query = "SELECT * FROM usuario WHERE usuario = '" + usuario + "'";
        var result = conexionDB.selectGeneral(query);

        if (result != "0") {
            return 1;
        } else {
            return 0;
        }
    }

    private string getDataUserSqlite(string usuario) {
        string query = "SELECT * FROM usuario WHERE usuario = '" + usuario + "'";
        var result = conexionDB.selectGeneral(query);

        return result;
    }

    private int updateUserSqlite(string usuario, string nombre, string rol, string gradoEstudios, string programa, int status) {
        string query = "UPDATE usuario SET usuario = '" + usuario + "', nombre = '" + nombre + "', rol = '" + rol + "', gradoEstudios = '" + gradoEstudios + "', programa = '" + programa + "', status = " + status + ", WHERE usuario = '" + usuario + "'";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }

    private int saveLogSqlite(string fechaInicio, string fechaTermino, string dispositivo, int syncroStatus, string idCodigo, string idUsuario) {
        string query = "INSERT INTO log (fechaInicio, fechaTermino, dispositivo, syncroStatus, idCodigo, idUsuario) VALUES ('" + fechaInicio + "', '" + fechaTermino + "', '" + SystemInfo.deviceModel + "', " + syncroStatus + ", " + idCodigo + ", " + idUsuario + ")";
        var result = conexionDB.alterGeneral(query);

        if (result == 1) {
            return 1;
        } else {
            return 0;
        }
    }
}
