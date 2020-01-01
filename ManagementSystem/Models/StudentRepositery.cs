using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class StudentRepositery
    {
        //database connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);

        //FOR STUDENT REGISTRATION
        
        /// <summary>
        /// method to register new student and inserting detail of student ino student table
        /// </summary>
        /// <param name="sr"></param>
        /// <returns>student id</returns>
        public int StudentCreate(Student sr)
        {
            
            //to generate global uniqe identefier
            Guid guid = Guid.NewGuid();
            int i = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("student_Registration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@s_id", sr.s_id);
                cmd.Parameters.AddWithValue("@s_name",sr.s_name);
                cmd.Parameters.AddWithValue("@s_phone", sr.s_phone);
                cmd.Parameters.AddWithValue("@s_email", sr.s_email);
                cmd.Parameters.AddWithValue("@s_password", sr.s_password);
                cmd.Parameters.AddWithValue("@s_branch_code", sr.s_branch_code);
                cmd.Parameters.AddWithValue("@guid", guid.ToString());

                //To read id from database
                cmd.Parameters.Add("@s_id", SqlDbType.Int);
                cmd.Parameters["@s_id"].Direction = ParameterDirection.Output;
                con.Open();
                i = cmd.ExecuteNonQuery();
                sr.s_id = Convert.ToInt32(cmd.Parameters["@s_id"].Value);
                con.Close();

            }
            catch(Exception ex)
            {
                ExceptionRepsitery dblayerEeception = new ExceptionRepsitery();
                int result = dblayerEeception.ExceptionInsert(ex);
                if (result == 0)
                {
                    dblayerEeception.InsertExceptionIntoLogFile(ex);
                };
            }
            return sr.s_id;
        }

       



        /// <summary>
        /// Read data of student from database from student table using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>student detail on refrence sr</returns>
        public Student ReadStudentById(int id)
        {
            int i = 0;
            Student sr = new Student();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM student WHERE s_id=" + id, con);

                //Code to read the data rows from database
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    sr.s_id = Convert.ToInt32(dr["s_id"]);
                    sr.s_name = Convert.ToString(dr["s_name"]);
                    sr.s_email = Convert.ToString(dr["s_email"]);
                    sr.s_phone = Convert.ToString(dr["s_phone"]);
                    sr.guid = Convert.ToString(dr["guid"]);
                    sr.s_branch_code = Convert.ToString(dr["s_branch_code"]);

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

        /// <summary>
        /// Read data of student from database from student table using guid 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>student detail on refrence sr</returns>
        public Student ReadStudentByGuid(string guid)
        {
            int i = 0;
            Student sr = new Student();
            try
            {
                //Hear passing guid as string is imprtant
                SqlCommand cmd = new SqlCommand("SELECT * FROM student WHERE guid='" + guid + "'", con);

                //Code to read the data rows from database
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    sr.s_id = Convert.ToInt32(dr["s_id"]);
                    sr.s_name = Convert.ToString(dr["s_name"]);
                    sr.s_email = Convert.ToString(dr["s_email"]);
                    sr.s_phone = Convert.ToString(dr["s_phone"]);
                    sr.guid = Convert.ToString(dr["guid"]);
                    sr.s_password = Convert.ToString(dr["s_password"]);

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
            return sr;
        }
        
        /// <summary>
        /// activateAccount method to make active coloumn 1 as registration verified in student table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return 1 as success after activation</returns>
        public int activateAccount(string id)
        {
            int success = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("student_MakingActiveTrue", con);
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
        public List<Student> ReadAllStudentDetail()
        {

            List<Student> studentList = new List<Student>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from student", con);                

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();
                foreach(DataRow dr in dt.Rows)
                {
                    studentList.Add(

                                            new Student
                                            {
                                                s_id = Convert.ToInt32(dr["s_id"]),
                                                s_name = Convert.ToString(dr["s_name"]),
                                                s_email = Convert.ToString(dr["s_email"]),
                                                s_phone = Convert.ToString(dr["s_phone"]),
                                                s_branch_code = Convert.ToString(dr["s_branch_code"]),
                                                createdOn = Convert.ToDateTime(dr["createdOn"]),
                                                modifiedOn = Convert.ToDateTime(dr["modifiedOn"]),
                                                //deletedOn = Convert.ToDateTime(dr["deletedOn"]),
                                                isdeleted = Convert.ToBoolean(dr["isdeleted"]),
                                                guid = Convert.ToString(dr["guid"]),
                                                active = Convert.ToBoolean(dr["active"])

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
            return studentList;
        }
        //===========================Validate student email if given email is allready registered then reject registration===================
        public int IsStudentAllReadyRegistered(string email)
        {
            SqlCommand cmd = new SqlCommand("Select s_id from Student where s_email='" + email + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);            
            con.Close();
            return dt.Rows.Count;
           
        }
        //================================================================================================================================




    }
}