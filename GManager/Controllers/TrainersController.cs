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
    [RoutePrefix("Trainers")]
    public class TrainersController : ApiController
    {
        private Dictionary<string, string> _Trainers = new Dictionary<string, string>();
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";

        [Route("add/{FirstName}")]
        [HttpPost()]
        public string AddTrainers(string FirstName, [FromBody] object details)
        {
            var strparse2 = JObject.Parse(details.ToString());
            var LastName = (string)strparse2.SelectToken("LastName");
            var Gender = (string)strparse2.SelectToken("Gender");
            var PhoneNumber = (string)strparse2.SelectToken("PhoneNumber"); 
            var UserName = (string)strparse2.SelectToken("UserName");
            var Email = (string)strparse2.SelectToken("Email");

            String strPost = "INSERT INTO Trainers VALUES('" + FirstName + "' , '" + LastName + "' , '" + Gender + "' , '" + PhoneNumber + "' , '" + Email + "' , '" + UserName + "')";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strPost), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0} added to the Trainers!", FirstName);
        }
        
        [Route("{UserName}/{FirstName}/{LestName}/{Gender}/{PhoneNumber}")]
        [HttpPost()]
        public string updateTrainers(string UserName, string FirstName, string LestName, string Gender, string PhoneNumber,[FromBody] object Email)
        {
            string source = Email.ToString();
            dynamic data = JObject.Parse(source);
            String strGetNew = "UPDATE Trainers SET FirstName = '" + FirstName + "', LestName = '" + LestName + "',	Gender = '" + Gender + "',	PhoneNumber = '" + PhoneNumber + "',	Email = '" + data.email + "' WHERE UserName = '" + UserName + "';"; 
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strGetNew), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0}'s info updated!", FirstName);
        }

        [Route("all")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<Trainers>> GetAllTrainers()
        {
            String StrAll = "SELECT * FROM Trainers ORDER BY LestName;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<Trainers> list = new List<Trainers>();
            while (sqlDataReader.Read())
            {
                list.Add(new Trainers
                {
                    FirstName = sqlDataReader.GetString(0).ToString().Trim(),
                    LastName = sqlDataReader.GetString(1).ToString().Trim()
                });
            }


            sqlConnection.Close();
            return Json(list);
        }


        [Route("{FirstName}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<Trainers>> GetByFirstname(string FirstName)
        {
            string StrAll = "SELECT * FROM Trainers WHERE UserName = '" + FirstName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<Trainers> list = new List<Trainers>();
            while (sqlDataReader.Read())
            {
                list.Add(new Trainers
                {
                    FirstName = sqlDataReader.GetString(0).ToString().Trim(),
                    LastName = sqlDataReader.GetString(1).ToString().Trim(),
                    Gender = sqlDataReader.GetString(2).ToString().Trim(),
                    PhoneNumber = sqlDataReader.GetString(3).ToString().Trim(),
                    Email = sqlDataReader.GetString(4).ToString().Trim()
                });
            }

            sqlConnection.Close();
            return Json(list);
        }

        [Route("{UserName}")]
        [HttpDelete()]
        public string DeleteTrainers(string UserName)
        {
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(new string[]
            {
                "DELETE FROM Trainers WHERE UserName = '" + UserName + " '; "
            }), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            if (!this._Trainers.ContainsKey(UserName))
            {
                return UserName + " deleted.";
            }
            else
                return "UserName not found.";
        }
    }
}

