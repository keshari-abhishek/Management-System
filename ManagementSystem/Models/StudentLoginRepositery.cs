using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class StudentLoginRepositery
    {
        //database connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
        /// <summary>
        /// FOR STUDENT LOGIN AUTHENTICATION
        /// </summary>
        /// <param name="sl_email"></param>
        /// <param name="sl_password"></param>
        /// <returns></returns>
        public Student StudentLoginAuthentication(string sl_email,string sl_password)
        {
            int i = 0;
            int success = 0;
            Student sr = new Student();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM student WHERE s_email='"+sl_email+"'",con);
                
                //Code to read the data rows from database
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Open();
                foreach (DataRow dr in dt.Rows)
                {                    
                    sr.s_email = Convert.ToString(dr["s_email"]);
                    sr.s_password = Convert.ToString(dr["s_password"]);
                    sr.active= Convert.ToBoolean(dr["active"]);
                    sr.s_id = Convert.ToInt32 (dr["s_id"]);
                    sr.s_name = Convert.ToString(dr["s_name"]);
                    if (sl_email == sr.s_email && sl_password == sr.s_password && sr.active == true)
                    {
                        return sr;

                    }

                }
                i = cmd.ExecuteNonQuery();
                con.Close();
                
            }
            catch (Exception ex)
            {
                ExceptionRepsitery dblayerEeception = new ExceptionRepsitery();
                int result = dblayerEeception.ExceptionInsert(ex);
                if (result == 0)
                {
                    dblayerEeception.InsertExceptionIntoLogFile(ex);
                }
            }
            return sr;

        }
    }
}