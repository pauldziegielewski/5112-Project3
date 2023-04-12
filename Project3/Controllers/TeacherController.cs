using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project3.Models;
using System.Diagnostics;

namespace Project3.Controllers
{
    //The TeacherControllor will listen to REQUESTS and link to specific page that we want to render (List)
    public class TeacherController : Controller
    {




        /// <summary>
        /// Returns a dynamically rendered page of a list of all teachers from a database
        /// The teacher list is a list of links that you can click on that will take you to another page for more detail about that teacher
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <example>Alexander</example>
        /// <returns>Alexander Bennett</returns>
        //GET : /Teacher/List/{SearchKey}
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }






        /// <summary>
        /// Returns an dynamically rendered page that displays details of a particular teacher
        /// </summary>
        /// <param name="id">1</param>
        /// <returns>Alexander Bennett</returns>
        /// <returns>Hire date: 2016-08-05</returns>
        /// <returns>Salary: 55.30</returns>
        //Get : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
       

            return View(NewTeacher);
        }



        /// <summary>
        /// Takes you to a page where a user choice is required of whether to proceed with the deletion of a particular teacher (a choice to not proceed is also available with a "cancel" option)
        /// </summary>
        /// <param name="id">An id number of the teacher that is to be deleted is required</param>
        /// <example>id = 1</example>
        /// <returns>If "confirm delete" is selected then the teacher associated with Id=1 will be deleted (Alexander Bennett, Salary: 55.30, Hire date: 2016-08-05)</returns>

        //Get : /Teacher/ConfirmDelete/{id}
        public ActionResult ConfirmDelete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }






        //POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }







        /// <summary>
        /// Take you to a page where various input fields about a teacher are required before a new teacher can be added to a database  
        /// </summary>
        /// <example>input first Name: Rupaul</example> 
        /// <example>input last Name: Charles</example>
        /// <example>input Salary: 222.00</example>
        /// <example>input Hire Date 2000-11-08</example>
        /// <returns>Stored details associated with those input fields</returns>
        public ActionResult AddTeacher() {

            return View();
        }





        /// <summary>
        /// Take you to a page where various input fields about a teacher are required before a new teacher can be added to a database  
        /// </summary>
        /// <example>input first Name: Rupaul</example> 
        /// <example>input last Name: Charles</example>
        /// <example>input Salary: 222.00</example>
        /// <example>input Hire Date 2000-11-08</example>
        /// <returns>Stored details associated with those input fields</returns>
        //POST: /Teacher/CreateTeacher
        [HttpPost]
        public ActionResult CreateTeacher(string TeacherFname, string TeacherLname, decimal TeacherSalary, DateTime TeacherHireDate)
        {

            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(TeacherSalary);
            Debug.WriteLine(TeacherHireDate);
      

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname= TeacherFname;
            NewTeacher.TeacherLname= TeacherLname;
            NewTeacher.TeacherSalary = TeacherSalary;
            NewTeacher.TeacherHireDate = TeacherHireDate;

            TeacherDataController controller = new TeacherDataController();
            controller.AddNewTeacher(NewTeacher);
          
            return RedirectToAction("List");

        }


    }
}