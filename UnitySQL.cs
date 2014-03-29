using System;
using Mono.Data.SqliteClient;

namespace UnitySQL {

    public static class UnitySQL {

        private static string _uri = "URI=file:SQLite.db";
        private static string _dbName = "SQLite";

        private static IDbConnection _dbcon;



        public static void SetURI(string value) {
            _uri = value; 
            _dbName = null;
        }

        public static string GetURI() {
            return _uri;
        }



        public static void SetDbName(string value) {
            _uri = "URI=file:" + value + ".db";
            _dbName = value;
        }

        public static string GetDbName() {
            return _dbName;
        }



        //Sets up the database
        public static void Setup() {
            _dbcon = new IDbConnection(_uri);
        }
    }
}
