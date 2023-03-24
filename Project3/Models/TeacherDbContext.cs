using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MySql.Data.MySqlClient;

namespace Project3.Models
{
    //this class will store the information retured in order to access the database
    public class TeacherDbContext
    {
        /// <summary>
        /// Returns a connection to the Blog database
        /// </summary>
        /// <example>private TeacherDbContext Blog = new TeacherDbContext()
        /// MySqlConnection Conn = Blog.AccessDatabase()</example>
        /// <returns>A MySqlConnection Object</returns>
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "blog"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        protected static string ConnectionString
        {

            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password;
            }
        }

        //This is the only method in this class (TeacherDbContext)
        //This method is public = it can be accessed by any controller which exists in the webserver
        //an object = an instantiation of a class
        // "MySqlConnection" is a class that connects MySQL database to .NET
        public MySqlConnection AccessDatabase()
        {
            //We are instantiating the MySqlConnection Class to create an object
            //The object is a specific connetion to the BLOG database on port 3306 of localhost
            // CLASS/Model = humans (Metaphor)
            // OBJECT = human (Metaphor)
            // Class ==> Object ==> Method
            return new MySqlConnection(ConnectionString);
        }

    }
}