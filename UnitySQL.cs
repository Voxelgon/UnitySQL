using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public static void Query(string query, Dictionary<string, string> parameters) {
            IDbCommand _cmd = _dbcon.CreateCommand();
            _cmd.CommandText = query;

            foreach(string _param in parameters) {
                _cmd.Parameters.Add(new SqliteParameter(_param, parameters[_param]);
            }

            IDataReader _reader = _cmd.ExecuteReader();
            _reader.Close();
        }



        public static IDataReader QueryAsReader(string query) {
            IDbCommand _cmd = _dbcon.CreateCommand();
            _cmd.CommandText = query;

            return _cmd.ExecuteReader();
        }

        public static IDataReader QueryAsReader(string query, Dictionary<string, string> parameters) {
            IDbCommand _cmd = _dbcon.CreateCommand();
            _cmd.CommandText = query;

            foreach(string _param in parameters) {
                _cmd.Parameters.Add(new SqliteParameter(_param, parameters[_param]);
            }

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

        public statidc List<Dictionary<string, object>> QueryAsList(string query) {
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

            foreach(string _param in parameters) {
                _cmd.Parameters.Add(new SqliteParameter(_param, parameters[_param]);
            }

            _reader.Close();
            return _list;
        }



        private static string ReadFile(string path) {
            StreamReader _sr = new StreamReader(path);
            string contents;

            try {
                contents = _sr.ReadToEnd();
            }
            catch(System.IO.IOException exception) {
                return null;
            }

            return contents;
        }


        public static void RunFile(string path) {
            string _query = ReadFile(path);
            Query(_query);
        }

        public static void RunFile(string path, Dictionary<string, string> parameters) {
            string _query = ReadFile(path);
            Query(_query, parameters);
        }



        public static IDataReader RunFileAsReader(string path) {
            string _query = ReadFile(path);

            return QueryAsReader(_query);
        }

        public static IDataReader RunFileAsReader(string path, Dictionary<string, string> parameters) {
            string _query = ReadFile(path);

            return QueryAsReader(_query, parameters);
        }



        public static List<Dictionary<string, object>> RunFileAsList(string path) {
            string _query = ReadFile(path);

            return QueryAsList(_query);
        }

        public static List<Dictionary<string, object>> RunFileAsList(string path, Dictionary<string, string> parameters) {
            string _query = ReadFile(path);

            return QueryAsList(_query, parameters);
        }
    }
}
