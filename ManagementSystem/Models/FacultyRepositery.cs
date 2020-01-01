using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    //FOR FACULTY REGISTRATION
    public class FacultyRepositery
    {
        
        //database connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);

        /// <summary>
        /// method to register new faculty and inserting detail of facuty ino faculty table
        /// </summary>
        /// <param name="fr"></param>
        /// <returns>registered faculty id</returns>
        public int FacultyCreate(Faculty fr)
        {
            //to generate global uniqe identefier
            Guid guid = Guid.NewGuid();
            int i = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("faculty_Registration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@f_name", fr.f_name);
                cmd.Parameters.AddWithValue("@f_phone", fr.f_phone);
                cmd.Parameters.AddWithValue("@f_email", fr.f_email);
                cmd.Parameters.AddWithValue("@f_password", fr.f_password);
                cmd.Parameters.AddWithValue("@f_subject_code", fr.f_subject_code);
                //cmd.Parameters.AddWithValue("@f_subject_code","1234");
                cmd.Parameters.AddWithValue("@guid", guid.ToString());

                //To read id from database
                cmd.Parameters.Add("@f_id", SqlDbType.Int);
                cmd.Parameters["@f_id"].Direction = ParameterDirection.Output;
                con.Open();
                i = cmd.ExecuteNonQuery();
                fr.f_id = Convert.ToInt32(cmd.Parameters["@f_id"].Value);
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
            return fr.f_id;
        }
        
        /// <summary>
        /// Read data of faculty from database from faculty table using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return the detail of the particular id</returns>
        public Faculty ReadFacultyById(int id)
        {
            int i = 0;
            Faculty fr = new Faculty();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM faculty WHERE f_id=" + id, con);

                //Code to read the data rows from database
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    fr.f_id = Convert.ToInt32(dr["f_id"]);
                    fr.f_name = Convert.ToString(dr["f_name"]);
                    fr.f_email = Convert.ToString(dr["f_email"]);
                    fr.f_phone = Convert.ToString(dr["f_phone"]);
                    fr.guid = Convert.ToString(dr["guid"]);
                    fr.f_subject_code = Convert.ToString(dr["f_subject_code"]);
                }
                i = cmd.ExecuteNonQuery();
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
            return fr;
        }

        /// <summary>
        /// Read data of faclty from database from faculty table using guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>return the detail of the particular id</returns>
        public Faculty ReadFacultyByGuid(string guid)
        {
            int i = 0;
            Faculty fr = new Faculty();
            try
            {
                //Hear passing guid as string is imprtant
                SqlCommand cmd = new SqlCommand("SELECT * FROM faculty WHERE guid='" + guid + "'", con);

                //Code to read the data rows from database
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    fr.f_id = Convert.ToInt32(dr["f_id"]);
                    fr.f_name = Convert.ToString(dr["f_name"]);
                    fr.f_email = Convert.ToString(dr["f_email"]);
                    fr.f_phone = Convert.ToString(dr["f_phone"]);
                    fr.guid = Convert.ToString(dr["guid"]);
                    fr.f_password = Convert.ToString(dr["f_password"]);

                }
                i = cmd.ExecuteNonQuery();
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
            return fr;
        }

        /// <summary>
        /// activateAccount method to make active coloumn 1 as registration verified in faculty table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return 1 after making active field as 1</returns>
        public int activateAccount(string id)
        {
            int success = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("faculty_MakingActiveTrue", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                success = cmd.ExecuteNonQuery();
                da.Fill(dt);
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
            return success;
        }
        public int IsFacultyAllReadyRegistered(string email)
        {
            SqlCommand cmd = new SqlCommand("select * from faculty where f_email='" + email+"'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            return dt.Rows.Count;
        }
    }
}