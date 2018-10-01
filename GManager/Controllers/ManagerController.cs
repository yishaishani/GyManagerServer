using GyManagerAPI.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GManager.Controllers
{
    [RoutePrefix("Manager")]
    public class ManagerController : ApiController
    {
        private Dictionary<string, string> _Manager = new Dictionary<string, string>();
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";

        [Route("add/{FirstName}")]
        [HttpPost()]
        public string AddManager(string FirstName, [FromBody] object details)
        {
            var strparse2 = JObject.Parse(details.ToString());
            var LastName = (string)strparse2.SelectToken("LastName");
            var PhoneNumber = (string)strparse2.SelectToken("PhoneNumber");
            var UserName = (string)strparse2.SelectToken("UserName");
            var Email = (string)strparse2.SelectToken("Email");

            String strPost = "INSERT INTO Manager VALUES('" + FirstName + "' , '" + LastName + "' , '" + PhoneNumber + "' , '" + Email + "' , '" + UserName + "')";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strPost), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0} added to the Manager!", FirstName);
        }

        [Route("{UserName}/{FirstName}/{LestName}/{PhoneNumber}")]
        [HttpPost()]
        public string updateManager(string UserName, string FirstName, string LestName, string PhoneNumber, [FromBody] object Email)
        {
            string source = Email.ToString();
            dynamic data = JObject.Parse(source);
            String strGetNew = "UPDATE Manager SET FirstName = '" + FirstName + "', LastName = '" + LestName + "',	PhoneNumber = '" + PhoneNumber + "',	Email = '" + data.email + "' WHERE UserName = '" + UserName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strGetNew), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0}'s info updated!", FirstName);
        }

        [Route("all")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<Manager>> GetAllManager()
        {
            String StrAll = "SELECT * FROM Manager ORDER BY LastName;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<Manager> list = new List<Manager>();
            while (sqlDataReader.Read())
            {
                list.Add(new Manager
                {
                    FirstName = sqlDataReader.GetString(0).ToString().Trim(),
                    LastName = sqlDataReader.GetString(1).ToString().Trim()
                });
            }

            sqlConnection.Close();
            return Json(list);
        }

        [Route("{UserName}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<Manager>> GetByUsername(string UserName)
        {
            string StrAll = "SELECT * FROM Manager WHERE UserName = '" + UserName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<Manager> list = new List<Manager>();
            while (sqlDataReader.Read())
            {
                list.Add(new Manager
                {
                    FirstName = sqlDataReader.GetString(0).ToString().Trim(),
                    LastName = sqlDataReader.GetString(1).ToString().Trim(),
                    PhoneNumber = sqlDataReader.GetString(2).ToString().Trim(),
                    Email = sqlDataReader.GetString(3).ToString().Trim(),
                    UserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }

            sqlConnection.Close();
            return Json(list);
        }

        [Route("{UserName}")]
        [HttpDelete()]
        public string DeleteManager(string UserName)
        {
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(new string[]
            {
                "DELETE FROM Manager WHERE UserName = '" + UserName + " '; "
            }), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            if (!this._Manager.ContainsKey(UserName))
            {
                return UserName + " deleted.";
            }
            else
                return "UserName not found.";
        }
    }
}

