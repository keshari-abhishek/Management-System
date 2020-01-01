using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class SubjectRepositery
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
        /// <summary>
        /// To read the subject record from subject table from database
        /// </summary>
        /// <returns>return the list of all subject and there subject code</returns>
        public List<Subject> GetSubject()
        {

            List<Subject> subjectlist = new List<Subject>();
            try
            {
                Subject b = new Subject();
                SqlCommand cmd = new SqlCommand("subject_Read", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                cmd.ExecuteNonQuery();
                da.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    subjectlist.Add(new Subject
                    {
                        s_id = Convert.ToString(dr["s_id"]),
                        s_name = Convert.ToString(dr["s_name"])
                    });

                }


            }
            catch (Exception ex)
            {
                string error = ex.Message;
                ExceptionRepsitery dblayerEeception = new ExceptionRepsitery();
                int result = dblayerEeception.ExceptionInsert(ex);
                if (result == 0)
                {
                    dblayerEeception.InsertExceptionIntoLogFile(ex);
                }
            }
            return subjectlist;

        }
    }
}