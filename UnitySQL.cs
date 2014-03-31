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


        public static void CreateTable(string name, string pKey, Column[] columns) {
            string _sql;

            _sql = "CEATE TABLE `" + name + "` (\n";
            foreach (Column _c in columns) {
                _sql = _sql + _c.sql + "\n";
            }
            _sql = _sql + "PRIMARY KEY (`" + pKey + "`)\n);";
        }

    }

    public class Column : object {
        public string name;
        public string type = "int";
        public int length;
        public int length2;
        public bool notNull;
        public object defaultValue = null;

        public string sql;

        public Column(string name, string type, int length = 0, bool notNull = false, object defaultValue = null) {
            BuildSQL();
        }

        public Column(string name, string type, int length = 0, int length2 = 0, bool notNull = false, object defaultValue = null) {
            BuildSQL();
        }

        public Column(string sql) {
        }


        public void BuildSQL() {
            string _typeArgs;
            string _notNull;
            string _defaultString;

            if ((length2 != null) && (length != null) && (length2 != 0) && (length != 0)) {
                _typeArgs = "(" + length.ToString() + "," + length2.ToString() + ")";
            } else if(((length2 == null) ||(length2 == 0)) && (length != null) && (length != 0)) {
                _typeArgs = "(" + length.ToString() + ")";
            } else {
                _typeArgs = "";
            }

            if (notNull == true) {
                _notNull = "NOT NULL";
            } else {
                _notNull = "";
            }

            if(defaultValue == null) {
                _defaultString = "DEFAULT NULL";
            } else {
                _defaultString = "DEFAULT '" + defaultValue.ToString() + "'";
            }

            sql = "`" + name + "` " + type + _typeArgs + " "  + _notNull + " " + _defaultString + ",";

        }
    }
}
