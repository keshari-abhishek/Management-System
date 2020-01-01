using ManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Management System Application Description.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Feel free to contact.";

            return View();
        }
        //=============================================================Student registrarion=================================================================================
        [HttpGet]
        public ActionResult Create()
        {
            BranchRepositery read = new BranchRepositery();
            List<Branch> br = read.GetBranch();
            return View(br);
        }
        [HttpPost]

        public ActionResult Create(Student sr)
        {
            StudentRepositery dblayerStudent = new StudentRepositery();
            int status = dblayerStudent.IsStudentAllReadyRegistered(sr.s_email);
            if (status ==0)
            {
                var v = dblayerStudent.StudentCreate(sr);
                if (v > 0)
                {
                    SendEmail(sr);
                }
                else
                {
                    // return ;
                }
            }
            else
            {
                TempData["Message"] = "Email Already Exists.";
                return RedirectToAction("Create");
            }
            return RedirectToAction("StudentLoginForm");
        }
        //===================================================================================================================================================================

        //===================================Student Email Verification======================================================================================================
        /// <summary>
        /// STUDENT EMAIL AND ACCOUNT CONFERMATION
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public ActionResult SendEmail(Student sr)
        {
            try
            {
                Student student = new Student();
                StudentRepositery msr = new StudentRepositery();
                student = msr.ReadStudentById(sr.s_id);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("abhishek.keshari@clanstech.com");
                mm.To.Add(sr.s_email);
                mm.Subject = "Registration Confermation";

                //Body of the email with email varification link
                mm.Body = string.Format("Dear User,<BR>please click the following link To activate your Account .<BR> <a href=http://" + Request.Url.Host + ":" + Request.Url.Port + Url.Action("ActivateAccount", "Home", new { id = student.guid }) + ">" + "Click Here to ActivateAccount" + "</a>");

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.clanstech.com";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                NetworkCredential nc = new NetworkCredential("abhishek.keshari@clanstech.com", "abhishek_1");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                ViewBag.Message = "Message has been sent Successfully";
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
            return View("Index");
        }
        //===================================================================================================================================================================

        //===============================set active as 1 in database to activate student account=============================================================================  
        /// <summary>
        /// method to make active=1 in student table after verifivcation of email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ActivateAccount(string id)
        {
            try
            {
                StudentRepositery repo = new StudentRepositery();

                Student sr = new Student();
                int success = 0;
                success = repo.activateAccount(id);
                if (success == 1)
                {
                    return View("StudentActivatePage");

                }
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
            return View();
        }
        //===================================================================================================================================================================


        //=======================================================student Mail confermation pages after  email verification===================================================
        public ActionResult StudentActivatePage()
        {
            return View("StudentActivatePage");
        }        
        //====================================================================================================================================================================

        // =====================================================================Student login=================================================================================
        [HttpGet]
        public ActionResult StudentLoginForm()
        {           
            return View();
        }
        [HttpPost]

        public ActionResult StudentloginForm(StudentLogin sl)
        {
            string email = sl.sl_email;
            string password = sl.sl_password;
            StudentLoginRepositery slr = new StudentLoginRepositery(); 

                          //calling student login authentication method to validate student login
            var mod = slr.StudentLoginAuthentication(email,password);
            if(mod.s_id>0)
            {
                //Useing session for student Id and Name to access these values during the session 
                Session["studentId"] = mod.s_id;
                Session["studentName"] = mod.s_name;
               
                int s_id = Convert.ToInt32(Session["studentId"]);               
                StudentRepositery sr = new StudentRepositery();               
                Student st = sr.ReadStudentById(s_id);
                ViewBag.student = st;

                //To access all the assignment for student dashboard
                AssignementRepositery ar = new AssignementRepositery();
                List<Assignement> assi = new List<Assignement>();
                assi = ar.ReadAllAssignement();
                ViewBag.assi = assi;
                if(Session["studentId"]!=null)
                {
                    return View("StudentDashboard");
                }
                else
                {
                    return View("Index");
                }
                
            }

            if (Session["studentId"] != null)
            {
                return View("StudentDashboard");
            }
            else
            {
                return View("Index");
            }

        }
        //====================================================================================================================================================================

        //=====================================STUDENT DASHBOARD==============================================================================================================
        [HttpGet]
        public ActionResult StudentDashboard()
        {
            return View("StudentDashboard");
        }
        //====================================================================================================================================================================

        //=====================================Upload file// Assignment submetion by student==================================================================================
        public string SubmitAssignment(HttpPostedFileBase file)
        {
            string message = "Error: Please try agen to submit your assignment";
            try
            {
                int sucess = 0;
                if (file.ContentLength > 0)
                {
                    int s_id = Convert.ToInt32(Session["studentid"]);
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles/SubmittedAssignment"), fileName);
                    file.SaveAs(path);

                    SqlCommand cmd = new SqlCommand("assignment_Submitted", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@s_id", s_id);
                    cmd.Parameters.AddWithValue("@path", path);
                    con.Open();
                    sucess = cmd.ExecuteNonQuery();
                    con.Close();
                    if(sucess!=0)
                    {
                        return message = "You have submitted your assignment successfully";
                    }
                    

                }
            }
            catch(Exception ex)
            {
                ExceptionRepsitery dblayerEeception = new ExceptionRepsitery();
                int result = dblayerEeception.ExceptionInsert(ex);
                if (result == 0)
                {
                    dblayerEeception.InsertExceptionIntoLogFile(ex);
                }                
            }
            return message;


        }
        //====================================================================================================================================================================

        //==============================================Faculty registration process==========================================================================================
        [HttpGet]
        public ActionResult CreateFaculty()
        {
            SubjectRepositery subject = new SubjectRepositery();
            List<Subject> sl = subject.GetSubject();
            return View(sl);
        }
        [HttpPost]

        public ActionResult CreateFaculty(Faculty fr)
        {
            FacultyRepositery dblayerFaculty = new FacultyRepositery();
            int status = dblayerFaculty.IsFacultyAllReadyRegistered(fr.f_email);
            if (status == 0)
            {
                var v = dblayerFaculty.FacultyCreate(fr);
                if (v > 0)
                {
                    SendEmail(fr);
                }
                else
                {
                    // return ;
                }
            }
            else
            {
                TempData["Message"] = "Email is allready registered";
                return RedirectToAction("CreateFaculty");
            }
            return RedirectToAction("FacultyLoginForm");
        }
        //====================================================================================================================================================================

        //====================================FACULTY send email  MEtHOD======================================================================================================
        /// <summary>
        /// Method to send an email to registered faculty for confermation and acount activation
        /// </summary>
        /// <param name="fr"></param>
        /// <returns></returns>
        public ActionResult SendEmail(Faculty fr)
        {
            try
            {
                Faculty faculty = new Faculty();
                FacultyRepositery mfr = new FacultyRepositery();
                faculty = mfr.ReadFacultyById(fr.f_id);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("abhishek.keshari@clanstech.com");
                mm.To.Add(fr.f_email);
                mm.Subject = "Registration Confermation";

                //Body of the email with email varification link
                mm.Body = string.Format("Dear User,<BR>please click the following link To activate your Account .<BR> <a href=http://" + Request.Url.Host + ":" + Request.Url.Port + Url.Action("ActivateFacultyAccount", "Home", new { id = faculty.guid }) + ">" + "Click Here to ActivateAccount" + "</a>");

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.clanstech.com";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                NetworkCredential nc = new NetworkCredential("abhishek.keshari@clanstech.com", "abhishek_1");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                ViewBag.Message = "Message has been sent Successfully";
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
            return View("Index");
        }
        //=====================================================================================================================================================================

        //====================================making active as 1 in faculty table as account confermation=====================================================================
        /// <summary>
        /// method to make active=1 in faculty table after verifivcation of email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ActivateFacultyAccount(string id)
        {
            try
            {
                FacultyRepositery repo = new FacultyRepositery();

                Faculty fr = new Faculty();
                int success = 0;
                success = repo.activateAccount(id);
                if (success == 1)
                {
                    return View("FacultyActivatePage");

                }
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
            return View();
        }
        //===========================================Faculty acount confermation page after making active fiel as 1 in database=========================================================================================================================
        public ActionResult FacultyActivatePage()
        {
            return View("FacultyActivatePage");
        }
        //====================================================================================================================================================================

        //===============================================================Faculty Login========================================================================================
        [HttpGet]
        public ActionResult FacultyLoginForm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FacultyloginForm(FacultyLogin fl,HttpPostedFileBase file)
        {
            try
            {
                string email = fl.fl_email;
                string password = fl.fl_password;
                FacultyLoginRepositery flr = new FacultyLoginRepositery();

                              //calling faculty login authentication method to validate faculty login
                var mod = flr.FacultyLoginAuthentication(email, password);
                if (mod.f_id > 0)
                {
                    //Useing session for faculty Id and Name to access these values during the session
                    Session["facultyId"] = mod.f_id;
                    Session["facultyName"] = mod.f_name;
                    
                    FacultyRepositery fr = new FacultyRepositery();
                    var faculty = fr.ReadFacultyById(mod.f_id);
                    

                    if (Session["facultyid"]!=null)
                    {
                        return RedirectToAction("FacultyDashboard");
                    }
                    else
                    {
                        return View("Index");
                    }
                    

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

            return View("FacultyLoginForm");

        }
        //===================================================================================================================================================================
       
        //=====================================fACULTY DASHBOARD==============================================================================================================

        [HttpGet]
        public ActionResult FacultyDashboard()
        {
            int id = Convert.ToInt32(Session["facultyId"].ToString());
            FacultyRepositery fr = new FacultyRepositery();
            StudentRepositery sr = new StudentRepositery();
            Faculty fac = fr.ReadFacultyById(id);//Accessing a particular faculty detail for dashboard
            ViewBag.fac = fac;
            
            List < Student > studentDetail = sr.ReadAllStudentDetail();//Accessing all student detail to show on faculty dashboard
            ViewBag.student= studentDetail;

            //To access all the uploaded assignment to check and give marks and remarks
            SubmittedAssignmentRepositery sar = new SubmittedAssignmentRepositery();
            List<SubmittedAssignment> subassList = new List<SubmittedAssignment>();
            subassList = sar.ReadAllSubmittedAssignment();
            ViewBag.subassList = subassList;            

            return View("FacultyDashboard");
        }
        [HttpPost]
        public ActionResult FacultyDashboard(SubmittedAssignment mark)
        {
            int f_id = Convert.ToInt32(Session["facultyId"]);
            SubmittedAssignmentRepositery obj = new SubmittedAssignmentRepositery();
            obj.InsertMarksAndReview(mark,f_id);
            
            

            return View("FacultyDashboard");
        }
        //=========================================================================================================================

        //=====================================Upload File// assignment uploade by faculty===================================================================================================================
        public string UploadFile(HttpPostedFileBase file)
        {
            string message = "Error: Please Try Agaain to Upload Your File";
            try
            {
                if (file.ContentLength > 0)
                {
                    int id = Convert.ToInt32(Session["facultyId"].ToString());

                    string fileName = Path.GetFileName(file.FileName);//Storing the file name of uploaded file in fileName variable
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(path);
                    ViewBag.Message = "File Uploadeed Successfuly";

                    //Inserting detail in database into assignment table                    
                    SqlCommand cmd = new SqlCommand("assignment_Created", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("f_id", id);
                    cmd.Parameters.AddWithValue("a_name", fileName);
                    cmd.Parameters.AddWithValue("path", path);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    message = "File Uploaded Successfuly";

                }
                return message;

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
                return message;
            }

        }
        //====================================================================================================================================================================
        
        //=====================================log out================================================================================
        public ActionResult LogOut()
        {
            Session.Clear();
            return View("Index");
        }
        //===============================================================================================================================
        

    }
}