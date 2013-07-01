using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace DsAuto.Helper.SQLHelper
{
    public class SQLITEHelper
    {
        SQLiteConnection conn;

        SQLiteCommand cmd = new SQLiteCommand();

        SQLiteConnectionStringBuilder constr;

        public void CreateDB(string dbName)
        {
            if (File.Exists(dbName))
                File.Delete(dbName);
            SQLiteConnection.CreateFile(dbName);
        }

        /// <summary>
        /// 返回sqlite连接
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetConn()
        {
            conn = new SQLiteConnection();
            return conn;
        }

        public void Init(string dbName)
        {
            CreateDB(dbName);

            constr = new SQLiteConnectionStringBuilder();
            constr.DataSource = dbName;
            constr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
            conn.ConnectionString = constr.ToString();
            //conn.Open();
        }

        public int ExcuteSql(string sql)
        {
            int result = 0;
            try
            {
                conn = GetConn();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;

                result = cmd.ExecuteNonQuery();
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public DataSet ExcuteDataSet(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = GetConn();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ds;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataSet ExcuteProc(string procName, ref SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = GetConn();
                cmd.CommandText = procName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(values);

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ds;
        }
    }
}
