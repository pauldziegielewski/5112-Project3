using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project3.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.CodeDom.Compiler;
using Mysqlx.Resultset;
using System.Xml.XPath;
using System.Web.Http.Cors;
using Org.BouncyCastle.Bcpg;

namespace Project3.Controllers
{
        //This controller will listen for HTTP requests
        //This controller is connected to "TeacherDbContext", which has access to the Blog Database
    public class TeacherDataController : ApiController
    {
       // Here "Blog" is an object that represents an instance of the "BlogDbContext" class
        private TeacherDbContext Blog = new TeacherDbContext();
      
        /// <summary>
        /// Returns a list of teachers from the Blog database
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>A list of teacher first and last names</returns>
    
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]

        ///<summary>Returns a teacher name based on the name that is inputed into a search input field</summary>
        ///<example>Alexande Bennett (the search field is not case sensistive and the search input may only include partial name eg. If a user types in Alex = All variations of that name will return (Alex, Alexa, Alexandra, Alexander)</example>
        ///<returns>Alexander Bennett</returns>
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            // STEP 1 - Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            // STEP 2 - Connect between web server and database
            Conn.Open();

            // STEP 3 - Obtain a new query for teacher database
            MySqlCommand cmd = Conn.CreateCommand();

            // STEP 4 
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower (@key) or lower (teacherlname) like lower(@key) or (concat (teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            // STEP 5 - Store query results into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // STEP 6 - Create an empty list of teachers
            List<Teacher> Teachers = new List<Teacher> { };

            // STEP 7 - Iterate each row of teachers
            while (ResultSet.Read())
            {
                // The code below grants access to teacher columns as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = ResultSet["teacherlname"].ToString();
                decimal TeacherSalary = (decimal)ResultSet["salary"];
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];

                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherHireDate = TeacherHireDate;


                // .Add(NewTeacher) adds teacher name to the empty list from STEP 6
                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            // Returns the list of teachers
            return Teachers;

        }

        /// <summary>
        /// Returns a teacher name base on an associated teacher I.D. that is specified by user
        /// </summary>
        /// <example>GET api/TeacherData/FindTeacher/{id}</example>
        /// <example>teacher_id (1) ===> returns Alexander Bennett</example>
        /// <param>{1}</param>
        /// <returns>First Name = Alexander</returns>
        /// <returns>Last Name = Bennett</returns>
        /// <returns>Hire Date = "2016-08-05"</returns>
        /// <returns>Salary = 55.30</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{teacherid}")]
        //FindTeacher() METHOD
        public Teacher FindTeacher(int teacherid)
        {
            Teacher NewTeacher = new Teacher();

            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where teacherid =@id";

            cmd.Parameters.AddWithValue("@id", teacherid);
            cmd.Prepare();
          
            MySqlDataReader ResultSet = cmd.ExecuteReader();

           

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"].ToString();
                decimal TeacherSalary = (decimal)ResultSet["salary"];
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherHireDate = TeacherHireDate; 

            }

            return NewTeacher;
        }



        /// <summary>
        /// Returns a list of courses from the blog database
        /// </summary>
        /// <returns>coursecode, coursename</returns>
        /// <example>http5101, Web Application Development</example>
        [HttpGet]
        [Route("api/TeacherData/ListClasses")]
        public IEnumerable<Course> ListClasses()
        {

         
            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Classes join Teachers on Classes.teacherid = Teachers.teacherid";

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Course> ClassesTaught = new List<Course>();

            while (ResultSet.Read())
            {
                int ClassId = (int)ResultSet["classid"];
                string ClassCode = (string)ResultSet["classcode"];
                string ClassName = (string)ResultSet["classname"];

                Course NewClassesTaught = new Course();

                NewClassesTaught.ClassId = ClassId;
                NewClassesTaught.ClassCode= ClassCode;
                NewClassesTaught.ClassName= ClassName;

                ClassesTaught.Add(NewClassesTaught);
            }
            Conn.Close();

            return ClassesTaught;
        }

        /// <summary>
        /// Takes you to a page where a user choice is required of whether to proceed with the deletion of a particular teacher (a choice to not proceed is also available with a "cancel" option)
        /// </summary>
        /// <param name="id">An id number of the teacher that is to be deleted is required</param>
        /// <example>id = 1</example>
        /// <returns>If "confirm delete" is selected then the teacher associated with Id=1 will be deleted (Alexander Bennett, Salary: 55.30, Hire date: 2016-08-05)</returns>
        /// <example>POST: /api/TeacherData/DeleteTeacher/1</example>
        [HttpPost]
      
        public void DeleteTeacher(int id)
        {

            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }



        /// <summary>
        /// This is a method that allows the C part of CRUD aka creation of a new teacher record inside a database
        /// </summary>
        /// POST: api/teacherdata/addnewteacher
        /// <example>
        /// curl -d "" http://localhost:52305/api/teacherdata/addnewteacher
        /// </example>
        /// <param name="NewTeacher"></param>
        [HttpPost]
        //EnableCors stands for (Cross-origin resource sharing - CORS). The purpose of CORS is to allow the method to be called from any domain
        [EnableCors(origins:"*", methods:"*",headers:"*")]
        public void AddNewTeacher([FromBody]Teacher NewTeacher)
        {

            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Insert into teachers (teacherfname, teacherlname, salary, hiredate) values (@TeacherFname, @TeacherLname, @TeacherSalary, @TeacherHireDate)";

            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);

            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);

            cmd.Parameters.AddWithValue("@TeacherSalary", NewTeacher.TeacherSalary);

            cmd.Parameters.AddWithValue("@TeacherHireDate", NewTeacher.TeacherHireDate);


            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();





        }

        /// <summary>
        /// This is a public method that returns a string
        /// </summary>
        /// <returns>A string "this is a test"</returns>
        [HttpGet]
        [Route("api/TeacherData/test")]
        public string Test()
        {
            return "this is a test";
        }


        /// <summary>
        /// This is a public method - a public method is used when you need to send data to the server in order to: create, update, delete
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/teacherdata/test")]
        public string test()
        {
            return "this is a post test";
        }



        /// <summary>
        /// This method updates a teacher in the MySql database. 
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that maps to the teacher's table</param>Id of the teacher to be updated
        /// <param name="TeacherFname"></param>Id of the teacher to be updated
        /// <param name="TeacherLname"></param>First name of the teacher to be updated
        /// <param name="TeacherSalary"></param>Salary of the teacher to be updated
        /// <param name="TeacherHireDate"></param>Hire date of the teacher to be updated
        /// <returns>A dynamic page which provides the current info on a teacher</returns>
        /// <example>POST: api/TeacherData/UpdateTeacher/12
        /// FORM DATA / POST DATA / REQUEST BODY
        /// {
        /// "TeacherFname":"Grace",
        /// "TeacherLname":"Adler",
        /// "TeacherSalary": 65,
        /// "TeacherHireDate": 2000-04-22
        /// }
        /// </example>
        /// 
        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            //Store blog database connection into Conn
            MySqlConnection Conn = Blog.AccessDatabase();

            // Open Conn
            Conn.Open();

            // attach a command method to Conn and store it into cmd
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "UPDATE teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, salary=@TeacherSalary, hiredate=@TeacherHireDate where teacherid=@TeacherId";

            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherSalary", TeacherInfo.TeacherSalary);
            cmd.Parameters.AddWithValue("@TeacherHireDate", TeacherInfo.TeacherHireDate);
            cmd.Parameters.AddWithValue("@TeacherId", id);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

    }
}