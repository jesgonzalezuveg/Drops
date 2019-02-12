using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Object = System.Object;

public class conexionDB : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IDbConnection crearConexionDB() {
        string conn = "URI=file:" + Application.dataPath + "/Development/Jesus/Plugins/prueba.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        return dbconn;
    }

    private IDbCommand crearComandoDB(IDbConnection conexion,string query) {
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

    private int alterGeneral(string query) {

        IDbConnection dbconn= crearConexionDB();
        IDbCommand dbcmd = crearComandoDB(dbconn, query);
        var result = dbcmd.ExecuteNonQuery();

        cerrarConexionDB(dbconn, dbcmd);

            return result;
    }

    private String selectGeneral(string query) {
        IDbConnection dbconn = crearConexionDB();
        IDbCommand dbcmd = crearComandoDB(dbconn, query);
        IDataReader reader = dbcmd.ExecuteReader();
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
        cerrarConexionDB(dbconn, dbcmd);

        return json;
    }
}
