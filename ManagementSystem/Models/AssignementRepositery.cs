using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;

namespace ManagementSystem.Models
{
    public class AssignementRepositery
    {
        /// <summary>
        /// read assignment detail to show the assignment deati to student and download option for assignment download
        /// </summary>
        /// <returns></returns>
        public List<Assignement> ReadAllAssignement()
        {
            List<Assignement> assi = new List<Assignement>();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Select * from Assignment", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();
                foreach(DataRow dr in dt.Rows)
                {
                    assi.Add(
                                new Assignement
                                {
                                    a_id = Convert.ToInt32(dr["a_id"]),
                                    a_name = Convert.ToString(dr["a_name"]),
                                    path = Convert.ToString(dr["path"]),
                                    createdOn=Convert.ToDateTime(dr["createdOn"])

                                }

                            );
                }
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                ExceptionRepsitery dblayerEeception = new ExceptionRepsitery();
                int result = dblayerEeception.ExceptionInsert(ex);
                if (result == 0)
                {
                    dblayerEeception.InsertExceptionIntoLogFile(ex);
                }
            }
            return assi;


        }

    }
}