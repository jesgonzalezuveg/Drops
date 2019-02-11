using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Random = System.Random;

public class db : MonoBehaviour
{
    String code ="";
    String code2 ="";

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(WebServiceCodigo.insertarCodigo("XdKpla"));
        StartCoroutine(WebServiceCodigo.obtenerCodigo("XdKpla", 1));
        code = generateCode();
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
            return finalString;
        }
        else
        {
            Debug.Log("Error al insertar");
            return "Error al insertar";
        }
    }



    /** Función que sirve para guardar el códifo generado
    *
    *@param  chars Lista de caracteres 
    *@param  stringChars Contenedor de los 8 caracteres que contendra el codigo
    *@param  random Funcion para elección aleatoria
    *@param  finalString Código obtentido
    **/
    private int saveCode(string code)
    {
        string conn = "URI=file:" + Application.dataPath + "/Development/Jesus/Plugins/prueba.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('"+code+ "', 0, datetime(), datetime())";
        dbcmd.CommandText = sqlQuery;
        var result = dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;

        return result;
    }


    void sqlite_prueba()
    {
        string conn = "URI=file:" + Application.dataPath + "/Development/Jesus/Plugins/prueba.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM info";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string nombre = reader.GetString(1);
            int edad = reader.GetInt32(2);

            Debug.Log("id= " + id + "  nombre =" + nombre + "  edad =" + edad);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

}
