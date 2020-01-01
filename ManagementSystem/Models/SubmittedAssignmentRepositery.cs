using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;

namespace ManagementSystem.Models
{
    public class SubmittedAssignmentRepositery
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        public List<SubmittedAssignment> ReadAllSubmittedAssignment()
        {

            List<SubmittedAssignment> subassList = new List<SubmittedAssignment>();
            SqlCommand cmd = new SqlCommand("select * from SubmittedAssignment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach(DataRow dr in dt.Rows)
            {
                subassList.Add(
                                    new SubmittedAssignment
                                    {
                                        s_id = Convert.ToInt32(dr["s_id"]),
                                        //f_id = Convert.ToInt32(dr["f_id"]),
                                       a_id = Convert.ToInt32(dr["a_id"]),
                                        path = Convert.ToString(dr["path"]),
                                        // marks = Convert.ToInt32(dr["marks"]),
                                        // remarks = Convert.String(dr["remarks"])
                                    }
                               );
            }
            return subassList;
        }
        public void InsertMarksAndReview(SubmittedAssignment mark, int f_id)
        {
            try
            {
               // int facultyid = Convert.ToInt32(Session["facultyId"]);
                SqlCommand cmd = new SqlCommand("InsertMarksAndRemarks", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@marks", mark.marks);
                cmd.Parameters.AddWithValue("@remarks", mark.remarks);
                cmd.Parameters.AddWithValue("@f_id", f_id);
                cmd.Parameters.AddWithValue("@a_id",mark.a_id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
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
        }
        
    }
}