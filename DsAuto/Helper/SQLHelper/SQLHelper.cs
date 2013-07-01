using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DsAuto.Helper.SQLHelper
{
    public class SQLHelper    
    {        
        public static SqlConnection connection;
       
        public static SqlConnection Connection        
        {            
            get            
            {
                string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
                if (connection == null)                
                {                    
                    connection = new SqlConnection(connectionString);                    
                    connection.Open();               
                }                
                else if (connection.State == System.Data.ConnectionState.Closed)                
                {                    
                    connection = new SqlConnection(connectionString);                    
                    connection.Open();                
                }                
                else if (connection.State == System.Data.ConnectionState.Broken)                
                {                    
                    connection.Close();                   
                    connection.Open();                
                }                
                return connection;            
            }        
        }

        #region 增，删，改ExecuteNonQuery        
        /// <summary>        
        /// 单个数据增，删，改        
        /// </summary>        
        /// <param name="safeSql"></param>        
        /// <returns></returns>        
        ///
        public static int ExecuteCommand(string safeSql)        
        {            
            try            
            {               
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))                
                {                    
                    int result = cmd.ExecuteNonQuery();                    
                    return result;               
                }            
            }            
            catch (SqlException ex)            
            {                
                throw ex;            
            }        
        }        

        #endregion       

        #region 带参数的增，删，改ExecuteNonQuery        
        /// <summary>        
        /// 带多个参数的增，删，改        
        /// </summary>        
        /// <param name="safeSql"></param>        
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static int ExecuteCommand(string safeSql, params SqlParameter[] values)        
        {            
            try            
            {                
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))                
                {                    
                    cmd.Parameters.AddRange(values);                    
                    return cmd.ExecuteNonQuery();                
                }            
            }            
            catch (SqlException ex)            
            {                
                throw ex;           
            }     
        }        
        #endregion    
    
        #region 带参数的增，删，改ExecuteNonQuery       
        /// <summary>        
        /// 带多个参数的增，删，改       
        /// /// </summary>    
        /// /// <param name="safeSql"></param>      
        /// /// <param name="values"></param>    
        /// /// <returns></returns>     
        public static int ExecuteCommand(string safeSql, CommandType type, params SqlParameter[] values)    
        {      
            try      
            {         
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))          
                {               
                    cmd.CommandType = type;               
                    cmd.Parameters.AddRange(values);        
                    cmd.ExecuteNonQuery();                 
                    return cmd.ExecuteNonQuery();          
                }         
            }         
            catch (SqlException ex)  
            {             
                throw ex;    
            }     
        }       
        #endregion   

        #region 带参数的增，删，改ExecuteNonQuery (Special)  
        /// <summary>       
        /// 带多个参数的增，删，改       
        /// </summary>   
        /// <param name="safeSql"></param>       
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static int ExecuteCommand(string safeSql,CommandType type, int index)        
        {            
            try            
            {                
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))                
                {                    
                    cmd.CommandType = type;
                    SqlParameter paramOne = new SqlParameter("@rid", SqlDbType.Int); 
                    paramOne.Value = index;                
                    cmd.Parameters.Add(paramOne);          
                    return cmd.ExecuteNonQuery();               
                }      
            }           
            catch (SqlException ex)    
            {               
                throw ex;           
            }      
        }        
        #endregion       
        #region 查询语句ExecuteScalar  
        /// <summary>        
        /// 查单个值        
        /// </summary>        
        /// <param name="safeSql"></param>        
        /// <returns></returns>        
        public static int GetScalar(string safeSql)        
        {          
            try    
            {      
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))  
                {                
                    int result = Convert.ToInt32(cmd.ExecuteScalar());      
                    return result;         
                }         
            }          
            catch (SqlException ex)    
            {                        
                throw ex;          
            }
        }     
        #endregion   
        #region 带参数的查询语句ExecuteScalar       
        /// <summary>        
        /// 带参数的查询语句       
        /// </summary>        
        /// <param name="sql"></param>        
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static int GetScalar(string sql, params SqlParameter[] values)       
        {           
            try     
            {       
                using (SqlCommand cmd = new SqlCommand(sql, Connection))     
                {                 
                    cmd.Parameters.AddRange(values);     
                    int result = Convert.ToInt32(cmd.ExecuteScalar());    
                    return result;           
                }         
            }           
            catch (SqlException ex)   
            {            
                throw ex;   
            }       
        }        
        #endregion     
        
        #region 带执行类型的ExecuteScalar        
        /// <summary>       
        /// 带执行类型的ExecuteScalar        
        /// </summary>        
        /// <param name="sql"></param>        
        /// <param name="type"></param>        
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static int GetScalar(string sql, CommandType type, params SqlParameter[] values)        
        {            
            try            
            {                
                using (SqlCommand cmd = new SqlCommand(sql, Connection))                
                {                    
                    cmd.CommandType = type;                    
                    cmd.Parameters.AddRange(values);                    
                    int result = Convert.ToInt32(cmd.ExecuteScalar());                    
                    return result;               
                }       
            }           
            catch (SqlException ex)         
            {               
                throw ex;     
            }      
        }        
        #endregion        

        #region 返回DataReader       
        /// <summary>        
        /// 查询表,获取多个记录       
        /// </summary>        
        /// <param name="safeSql"></param>        
        /// <returns></returns>        
        public static SqlDataReader GetReader(string safeSql)        
        {      
            try  
            {       
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))    
                {              
                    SqlDataReader reader = cmd.ExecuteReader();    
                    return reader;          
                }                         
            }           
            catch (SqlException ex)   
            {            
                throw ex;  
            }     
        }        
        #endregion      
        
        #region 带参数DataReader        
        /// <summary>        
        /// 带参数的-查询表,获取多个记录        
        /// </summary>        
        /// <param name="sql"></param>        
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static SqlDataReader GetReader(string sql, params SqlParameter[] values)        
        {            
            try            
            {                
                using (SqlCommand cmd = new SqlCommand(sql, Connection))                
                {                    
                    cmd.Parameters.AddRange(values);                    
                    SqlDataReader reader = cmd.ExecuteReader();                    
                    return reader;                
                }            
            }            
            catch (SqlException)            
            {                
                throw;           
            }
        }        
        #endregion      

        #region 返回DataReader ,语句，类型，参数        
        /// <summary>        
        /// 查询表,获取多个记录---语句，类型，参数        
        /// </summary>        
        /// <param name="safeSql"></param>        
        /// <param name="cmdType"></param>        
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static SqlDataReader GetReader(string safeSql, CommandType cmdType, params SqlParameter[] values)        
        {            
            try            
            {                
                using (SqlCommand cmd = new SqlCommand(safeSql, Connection))                
                {                    
                    cmd.CommandType = cmdType;       
                    cmd.Parameters.AddRange(values);              
                    SqlDataReader reader = cmd.ExecuteReader();          
                    return reader;                
                }                           
            }            
            catch (SqlException ex)            
            {                               
                throw ex;           
            }                 
        }       
        #endregion       
        
        #region 返回dataTable      
        /// <summary>        
        /// 返回datatable        
        /// </summary>        
        /// <param name="safeSql"></param>       
        /// <returns></returns>        
        public static DataTable GetDataSet(string safeSql)   
        {         
            DataSet ds = new DataSet();     
            SqlCommand cmd = new SqlCommand(safeSql, Connection);   
            SqlDataAdapter da = new SqlDataAdapter(cmd);          
            da.Fill(ds);        
            return ds.Tables[0];      
        }        
        #endregion        

        #region 返回dataTable ,带参数        
        /// <summary>        
        ///  返回dataTable ,带参数使用        
        /// </summary>        
        /// <param name="sql"></param>       
        /// <param name="values"></param>        
        /// <returns></returns>        
        public static DataTable GetDataSet(string sql, params SqlParameter[] values)        
        {            
            DataSet ds = new DataSet();            
            SqlCommand cmd = new SqlCommand(sql, Connection);            
            cmd.Parameters.AddRange(values);            
            SqlDataAdapter da = new SqlDataAdapter(cmd);            
            da.Fill(ds);            
            return ds.Tables[0];        
        }        
        #endregion
    }    
}