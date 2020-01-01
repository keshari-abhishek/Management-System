using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class ExceptionRepsitery
    {
        //database connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);

        /// <summary>
        /// method to insert the exception details in the database in the exception table
        /// </summary>
        /// <param name="exdb"></param>
        /// <returns></returns>
        public int ExceptionInsert(Exception exdb)
        {
            int i = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("exception_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exceptionNumber", exdb.HResult.ToString());
                cmd.Parameters.AddWithValue("@message", exdb.Message.ToString());
                cmd.Parameters.AddWithValue("@method", exdb.TargetSite.ToString());                
                con.Open();
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
            return i;
        }

        /// <summary>
        /// method to write the exception in the LogFile if exception details are not inserted in exception table in databse
        /// </summary>
        /// <param name="ex"></param>
        public void InsertExceptionIntoLogFile(Exception ex)
        {
            try
            {
                var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"));
                filePath = filePath + "\\LogFile.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine();
                    writer.Write("Date = " + DateTime.Now.Date);
                    writer.WriteLine();
                    writer.Write("Error occured at method = " + ex.TargetSite.ToString());
                    writer.WriteLine();
                    writer.Write("Error code for the Exception is = " + ex.HResult.ToString());
                    writer.WriteLine();
                    writer.Write("Message for the exception = " + ex.Message.ToString());
                    writer.WriteLine();
                }
            }
            catch(Exception exc)
            {
                ExceptionRepsitery dblayerEeception = new ExceptionRepsitery();
                int result = dblayerEeception.ExceptionInsert(exc);
                if (result == 0)
                {
                    dblayerEeception.InsertExceptionIntoLogFile(exc);
                }
            }
        }
    }
}