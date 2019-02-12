using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Random = System.Random;
using System.Dynamic;
using Object = System.Object;

public class db : MonoBehaviour
{
    String code ="";
    String code2 ="";

    // Start is called before the first frame update
    void Start()
    {
        //"INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('test', 0, datetime(), datetime())"
        //"SELECT * FROM codigo"

        //sqlite_prueba();
        code = generateCode();
        //StartCoroutine(WebServiceCodigo.obtenerCodigo("XdKpla", 1));
        //StartCoroutine(WebServiceCodigo.insertarCodigo(code));
        GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text= code2;
    }

    // Update is called once per frame
    void Update()
    {

    }


    /** Funcion que sirve para generar un código de 8 caracteres de manera aleatoria
    *
    *@param  chars Lista de caracteres 
    *@param  stringChars Contenedor de los 6 caracteres que contendra el codigo
    *@param  random Funcion para elección aleatoria
    *@param  finalString Código obtentido
    **/
    public String generateCode()
    {
        var chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[6];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
            if (i == 2)
            {
                code2 = code2 + stringChars[i] + "-";
            }
            else
            {
                code2 = code2 + stringChars[i];
            }
        }

        var finalString = new String(stringChars);

        var result = saveCode(finalString);

        if (result == 1)
        {
            Debug.Log("Se inserto correctamente");
            Debug.Log(finalString);
            Debug.Log(result);
            return finalString;
        }
        else
        {
            Debug.Log("Error al insertar");
            Debug.Log(result);
            return "Error al insertar";
        }
    }

    private IDbConnection crearConexionDB() {
        string conn = "URI=file:" + Application.dataPath + "/Development/Jesus/Plugins/prueba.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        return dbconn;
    }

    private IDbCommand crearComandoDB(IDbConnection conexion, string query) {
        IDbCommand dbcmd = conexion.CreateCommand();
        //string sqlQuery = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('test', 0, datetime(), datetime())";
        string sqlQuery = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('test', 0, datetime(), datetime())";
        dbcmd.CommandText = sqlQuery;
        return dbcmd;
    }

    private void cerrarConexionDB(IDbConnection dbconn, IDbCommand dbcmd) {
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;
    }

    /** Función que sirve para guardar el códifo generado
    *
    *@param  chars Lista de caracteres 
    *@param  stringChars Contenedor de los 8 caracteres que contendra el codigo
    *@param  random Funcion para elección aleatoria
    *@param  finalString Código obtentido
    **/
    private int alterGeneral(string query) {

        IDbConnection dbconn = crearConexionDB();
        IDbCommand dbcmd = crearComandoDB(dbconn, query);
        var result = dbcmd.ExecuteNonQuery();

        cerrarConexionDB(dbconn, dbcmd);

        return result;
    }

    void sqlite_prueba()
    {
        string conn = "URI=file:" + Application.dataPath + "/Development/Jesus/Plugins/prueba.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM codigo";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        //List<String> firstList = new List<String>();
        int fieldCount;
        string json= "";
        while (reader.Read())
        {
            json = json + "{";
            //List<String> listToAdd = new List<String>();
            // Create a new dynamic ExpandoObject
            Object[] values = new Object[reader.FieldCount];
            fieldCount = reader.GetValues(values);
            for (int i = 0; i < fieldCount; i++) {
                //listToAdd.Add(reader.GetValue(i).ToString());
                if (i==(fieldCount-1)) {
                    json = json + "'" + reader.GetName(i).ToString() + "': '" + reader.GetValue(i).ToString() + "'";
                } else {
                    json = json + "'" + reader.GetName(i).ToString() + "': '" + reader.GetValue(i).ToString() + "', ";
                }
            }
            //firstList.AddRange(listToAdd);
            json = json + "},";
        }

        json = json.Remove(json.Length - 1);


        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
