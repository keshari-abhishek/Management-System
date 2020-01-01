using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class FacultyLoginRepositery
    {
        //database connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
        /// <summary>
        /// FOR LOGIN AUTHENTICATION OF FACULTY
        /// </summary>
        /// <param name="fl_email"></param>
        /// <param name="fl_password"></param>
        /// <returns></returns>
        public Faculty FacultyLoginAuthentication(string fl_email, string fl_password)
        {
            int i = 0;
            int success = 0;
            Faculty fr = new Faculty();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM faculty WHERE f_email='" + fl_email + "'", con);

                //Code to read the data rows from database
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    fr.f_email = Convert.ToString(dr["f_email"]);
                    fr.f_password = Convert.ToString(dr["f_password"]);
                    fr.active = Convert.ToBoolean(dr["active"]);
                    fr.f_id = Convert.ToInt32(dr["f_id"]);
                    fr.f_name = Convert.ToString(dr["f_name"]);
                    if (fl_email == fr.f_email && fl_password == fr.f_password && fr.active == true)
                    {                      
                        return fr;
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
            return fr;
        }
    }
}