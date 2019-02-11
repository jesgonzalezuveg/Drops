using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

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

    private int saveCode(string query, int tipo) {
        string conn = "URI=file:" + Application.dataPath + "/Development/Jesus/Plugins/prueba.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO codigo (descripcion, status, fechaRegistro, fechaModificacion) VALUES ('test', 0, datetime(), datetime())";
        dbcmd.CommandText = sqlQuery;

            var result = dbcmd.ExecuteNonQuery();

            dbcmd.Dispose();
            dbcmd = null;

            dbconn.Close();
            dbconn = null;

            return result;
    }
}
