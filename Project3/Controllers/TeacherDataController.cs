﻿using System;
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

namespace Project3.Controllers
{
    public class TeacherDataController : ApiController
    {

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

            // STEP 4 - SQL Query (DO I NEED AMNOTHER COMMANDTEXT TO SEARCH FOR SALARY, HIRE DATE?), WHAT DOES SITE.CSS/_LAYOUT.CSHTML CHANGE?
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
                string TeacherLname = (string)ResultSet["teacherlname"];
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
        [Route("api/TeacherData/FindTeacher/{id}")]
        //FindTeacher() METHOD
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where teacherid = " + id;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

           

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
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
    }
}