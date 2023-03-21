using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project3.Models;

namespace Project3.Controllers
{
    //The TeacherControllor will listen to REQUESTS and link to specific page that we want to render (List)
    public class TeacherController : Controller
    {


        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns a dynamic rendered page of a list of all teachers from a database
        /// The teacher list is a list of links that you can click on that will take you to another page for more detail of that teacher
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <example>Alexander</example>
        /// <returns>Alexander Bennett</returns>
        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        /// <summary>
        /// Returns an dynamically rendered feature page of a particular teacher that was clicked on
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
    }
}