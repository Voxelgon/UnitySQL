using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Data.SqliteClient;

namespace UnitySQL {

    public static class SQLite {

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



        public static int Count(string table, string column) {

            string sql = string.Format("SELECT COUNT(`{0}`) c FROM {1}", column, table);
            IDataReader reader = QueryAsReader(sql);

            reader.Read();
            int count = reader.GetInt32(0);

            return count;
        }



        public static void Query(string query) {
            IDbCommand cmd = _dbcon.CreateCommand();
            cmd.CommandText = query;
            IDataReader reader = cmd.ExecuteReader();
            reader.Close();
        }

        public static void Query(string query, Dictionary<string, string> parameters) {
            IDbCommand cmd = _dbcon.CreateCommand();
            cmd.CommandText = query;

            foreach(KeyValuePair<string, string> param in parameters) {
                cmd.Parameters.Add(new SqliteParameter(param.Key, param.Value));
            }

            IDataReader reader = cmd.ExecuteReader();
            reader.Close();
        }



        public static IDataReader QueryAsReader(string query) {
            IDbCommand cmd = _dbcon.CreateCommand();
            cmd.CommandText = query;

            return cmd.ExecuteReader();
        }

        public static IDataReader QueryAsReader(string query, Dictionary<string, string> parameters) {
            IDbCommand cmd = _dbcon.CreateCommand();
            cmd.CommandText = query;

            foreach(KeyValuePair<string, string> param in parameters) {
                cmd.Parameters.Add(new SqliteParameter(param.Key, param.Value));
            }

            return cmd.ExecuteReader();
        }



        public static List<Dictionary<string, object>> QueryAsList(string query) {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            IDataReader reader = QueryAsReader(query);
            DataColumnCollection columns = reader.GetSchemaTable().Columns;

            for (int r = 0; r < reader.FieldCount; r++) {
                reader.Read();

                Dictionary<string, object> record = new Dictionary<string, object>();

                foreach(DataColumn column in columns) {
                    record.Add(column.ColumnName, reader.GetValue(r));
                }

                list.Add(record);
            }

            reader.Close();
            return list;
        }

        public static List<Dictionary<string, object>> QueryAsList(string query, Dictionary<string, string> parameters) {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            IDataReader reader = QueryAsReader(query, parameters);
            DataColumnCollection columns = reader.GetSchemaTable().Columns;

            for (int r = 0; r < reader.FieldCount; r++) {
                reader.Read();

                Dictionary<string, object> record = new Dictionary<string, object>();

                foreach(DataColumn column in columns) {
                    record.Add(column.ColumnName, reader.GetValue(r));
                }

                list.Add(record);
            }
            reader.Close();
            return list;
        }



        private static string ReadFile(string path) {
            StreamReader sr = new StreamReader(path);
            string contents;

            try {
                contents = sr.ReadToEnd();
            }
            catch(System.IO.IOException exception) {
                return null;
            }

            return contents;
        }


        public static void RunFile(string path) {
            string query = ReadFile(path);
            Query(query);
        }

        public static void RunFile(string path, Dictionary<string, string> parameters) {
            string query = ReadFile(path);
            Query(query, parameters);
        }



        public static IDataReader RunFileAsReader(string path) {
            string query = ReadFile(path);

            return QueryAsReader(query);
        }

        public static IDataReader RunFileAsReader(string path, Dictionary<string, string> parameters) {
            string query = ReadFile(path);

            return QueryAsReader(query, parameters);
        }



        public static List<Dictionary<string, object>> RunFileAsList(string path) {
            string query = ReadFile(path);

            return QueryAsList(query);
        }

        public static List<Dictionary<string, object>> RunFileAsList(string path, Dictionary<string, string> parameters) {
            string query = ReadFile(path);

            return QueryAsList(query, parameters);
        }



        public static void Insert(string table, List<Dictionary<string, object>> data) {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO `" + table + "` (");

            if(data.Count == 0) {
                return;
            }

            int index = 0;
            foreach(KeyValuePair<string, object> pair in data[0]) {
                sql.Append(" `");
                sql.Append(pair.Key);
                sql.Append("`");

                if (index < (data[0].Count - 1)) {
                    sql.Append(",");
                }

                index++;
            }

            sql.Append(")\n");
            sql.Append("VALUES");

            index = 0;
            foreach(Dictionary<string, object> row in data) {
                index++;
                sql.Append("(");

                int index2 = 0;
                foreach(KeyValuePair<string, object> pair in row) {
                    sql.Append(" '");
                    sql.Append(pair.Value);
                    sql.Append("'");

                    if(index2 < (row.Count - 1)) {
                        sql.Append(",");
                    }

                    index2++;
                }

                sql.Append(")");

                if (index < (data.Count - 1)) {
                    sql.Append(",\n");
                }

                index++;
            }

            Query(sql.ToString());
        }
    }
}
