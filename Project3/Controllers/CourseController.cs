using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project3.Models;

namespace Project3.Controllers
{

    /// <summary>
    /// Returns a list of all courses taught by various teachers
    /// </summary>
    /// <returns>coursecode, coursename</returns>
    /// <example>http5101, Web Application Development</example>
    public class CourseController : Controller
    {
        // GET: /Course/ListCourse
        public ActionResult ListCourse()
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Course> ClassesTaught = controller.ListClasses();
            return View(ClassesTaught);
        }
    }
}