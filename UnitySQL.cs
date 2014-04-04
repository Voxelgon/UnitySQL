using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
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
            _dbcon = (IDbConnection) new SqliteConnection(_uri);
            _dbcon.Open();
        }


        public static void Query(string query) {
            IDbCommand _cmd = _dbcon.CreateCommand();
            _cmd.CommandText = query;
            IDataReader _reader = _cmd.ExecuteReader();
            _reader.Close();
        }


        public static IDataReader QueryAsReader(string query) {
            IDbCommand _cmd = _dbcon.CreateCommand();
            _cmd.CommandText = query;

            return _cmd.ExecuteReader();
        }


        public static List<Dictionary<string, object>> QueryAsList(string query) {
            List<Dictionary<string, object>> _list = new List<Dictionary<string, object>>();
            IDataReader _reader = QueryAsReader(query);
            DataColumnCollection _columns = _reader.GetSchemaTable().Columns;

            for (int r = 0; r < _reader.FieldCount; r++) {
                _reader.Read();

                Dictionary<string, object> _record = new Dictionary<string, object>();

                foreach(DataColumn _column in _columns) {
                    _record.Add(_column.ColumnName, _reader.GetValue(r));
                }

                _list.Add(_record);
            }

            _reader.Close();
            return _list;
        }
    }
}
