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
        sqlite_prueba();
        //code = generateCode();
        //StartCoroutine(WebServiceCodigo.obtenerCodigo("XdKpla", 1));
        //StartCoroutine(WebServiceCodigo.insertarCodigo(code));
        //GameObject.FindGameObjectWithTag("codigo").GetComponent<Text>().text= code2;
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
        string sqlQuery = "SELECT * FROM codigo";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        //List<Data> listCodes = new List<Data>();
        //Debug.Log(reader);
        dynamic output = new List<dynamic>();
        while (reader.Read())
        {
            // Create a new dynamic ExpandoObject
            dynamic row = new ExpandoObject();
            Object[] values = new Object[reader.FieldCount];
            int fieldCount = reader.GetValues(values);
            for (int i = 0; i < fieldCount; i++) {
                string name = reader.GetName(i);
                row.name = reader.GetValue(i);
            }

            output.Add(row);


            //Data data = new Data();

            /*data.id = reader.GetInt32(0);
            data.descripcion = reader.GetString(1);
            data.status = reader.GetInt32(2);
            data.fechaRegistro = reader.GetString(3);
            data.fechaModificacion = reader.GetString(4);
            listCodes.Add(data);*/
        }
        Debug.Log(output);

        foreach (var codigo in output) {
            Debug.Log(codigo.id);
            Debug.Log(codigo.descripcion);
            Debug.Log(codigo.status);
            Debug.Log(codigo.fechaRegistro);
            Debug.Log(codigo.fechaModificacion);
        }
       // return listCodes;*/


        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
