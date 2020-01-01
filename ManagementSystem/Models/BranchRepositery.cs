using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class BranchRepositery
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
        /// <summary>
        /// To read the branch record from branch table from database
        /// </summary>
        /// <returns>return the list of all branch and there branch code</returns>
        public List<Branch> GetBranch()
        {

            List<Branch> branchlist = new List<Branch>();
            try
            {
                Branch b = new Branch();
                SqlCommand cmd = new SqlCommand("branch_Read", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                cmd.ExecuteNonQuery();
                da.Fill(dt);                
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    branchlist.Add(new Branch {
                    b_id = Convert.ToString(dr["b_id"]),
                    b_name = Convert.ToString(dr["b_name"])
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
            return branchlist;

        }
    }
}