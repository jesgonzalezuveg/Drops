using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;
using Object = System.Object;

public class conexionDB {
    /** Funcion que sirve para generar la conexion a la base de datos
    *
    *@param  conn es la ruta donde se encuentra la base de datos local
    *@param  dbconn donde se crea el objeto conexion
    **/
    private IDbConnection crearConexionDB() {
#if UNITY_EDITOR
        string conn = "URI=file:" + Application.dataPath + "/Plugins/SQLite/pruebaAndroid.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        Debug.Log(dbconn.ConnectionString);
        return dbconn;
#endif
#if UNITY_ANDROID
        string p = "pruebaAndroid.db";
        string filepath = Application.persistentDataPath + "/" + p;
        if (!File.Exists(filepath)) {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
            while (!loadDB.isDone) { }
            File.WriteAllBytes(filepath, loadDB.bytes);
            createDB();
        }
        string connection;
        connection = "URI=file:" + filepath;
        Debug.Log("Stablishing connection to: " + connection);
        IDbConnection dbcon;
        dbcon = (IDbConnection)new SqliteConnection(connection);
        dbcon.Open(); //Open connection to the database.
        return dbcon;
#endif
    }

    private IDbCommand crearComandoDB(IDbConnection conexion, string query) {
        IDbCommand dbcmd = conexion.CreateCommand();
        //string sqlQuery = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('test', 0, datetime(), datetime())";
        string sqlQuery = query;
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
    public static int alterGeneral(string query) {
        conexionDB connect = new conexionDB();
        IDbConnection dbconn = connect.crearConexionDB();
        IDbCommand dbcmd = connect.crearComandoDB(dbconn, query);
        var result = dbcmd.ExecuteNonQuery();

        connect.cerrarConexionDB(dbconn, dbcmd);

        return result;
    }

    public static string selectGeneral(string query) {
        conexionDB connect = new conexionDB();
        IDbConnection dbconn = connect.crearConexionDB();
        IDbCommand dbcmd = connect.crearComandoDB(dbconn, query);
        IDataReader reader = dbcmd.ExecuteReader();
        //Debug.Log(reader.RecordsAffected);

        //List<String> firstList = new List<String>();
        int fieldCount;
        string json = "";
        while (reader.Read()) {
            json = json + "{";
            //List<String> listToAdd = new List<String>();
            // Create a new dynamic ExpandoObject
            Object[] values = new Object[reader.FieldCount];
            fieldCount = reader.GetValues(values);
            for (int i = 0; i < fieldCount; i++) {
                //listToAdd.Add(reader.GetValue(i).ToString());
                if (i == (fieldCount - 1)) {
                    //json = json + "'" + reader.GetName(i).ToString() + "': '" + reader.GetValue(i).ToString() + "'";
                    json = json + '\"' + reader.GetName(i).ToString() + '\"' + ": " + '\"' + reader.GetValue(i).ToString() + '\"';
                } else {
                    //json = json + "'" + reader.GetName(i).ToString() + "': '" + reader.GetValue(i).ToString() + "', ";
                    json = json + '\"' + reader.GetName(i).ToString() + '\"' + ": " + '\"' + reader.GetValue(i).ToString() + '\"' + ", ";
                }
            }
            //firstList.AddRange(listToAdd);
            json = json + "},";
        }

        if (json == "") {
            ////Debug.Log("No tiene datos");
            return "0";
        } else {
            ////Debug.Log("Tiene datos");
            json = json.Remove(json.Length - 1);


            reader.Close();
            reader = null;
            connect.cerrarConexionDB(dbconn, dbcmd);

            return json;
        }
    }

    private void createDB() {
        var query = "CREATE TABLE IF NOT EXISTS previousMessages (ID    INTEGER NOT NULL PRIMARY KEY , Cipher   VARCHAR(5000) NOT NULL, InitialMessage  VARCHAR(5000) NOT NULL,EncryptedMessage TEXT NOT NULL)";
        conexionDB connect = new conexionDB();
        IDbConnection dbconn = connect.crearConexionDB();
        IDbCommand dbcmd = connect.crearComandoDB(dbconn, query);
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Close();
        reader = null;
        connect.cerrarConexionDB(dbconn, dbcmd);
    }
}
