using GyManagerAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GManager.Controllers
{
   [RoutePrefix("Coaches")]
    public class CoachesController : ApiController
    {
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";
        private Dictionary<string, string> _CoachesNames = new Dictionary<string, string>();

        [Route("{FirstName}")]
        [HttpPost()]
        public string AddCoaches(string FirstName, [FromBody] object details)
        {
            var strparse2 = JObject.Parse(details.ToString());
            var LastName = (string)strparse2.SelectToken("LastName");
            var str2 = (string)strparse2.SelectToken("DayOfWork");
            var UserName = (string)strparse2.SelectToken("UserName");
            Boolean Sunday = false;
            Boolean Monday = false;
            Boolean Tuesday = false;
            Boolean Wednesday = false;
            Boolean Thursday = false;
            Boolean Friday = false;
            Boolean Saturday = false;
            int size = str2.Length;
            for (int i = 0; i < size; i++)
                switch (str2[i])
                {
                    case 'a':
                        Sunday = true;
                        break;
                    case 'b':
                        Monday = true;
                        break;
                    case 'c':
                        Tuesday = true;
                        break;
                    case 'd':
                        Wednesday = true;
                        break;
                    case 'e':
                        Thursday = true;
                        break;
                    case 'f':
                        Friday = true;
                        break;
                    case 'g':
                        Saturday = true;
                        break;
                }
            String strPost = "INSERT INTO Coaches VALUES('" + FirstName + "' , '" + LastName + "' , '" + Sunday + "' , '" + Monday + "' , '" + Tuesday + "' , '" + Wednesday + "' , '" + Thursday + "' , '" + Friday + "' , '" + Saturday + "' , '" + UserName + "')";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strPost), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0} added to the Coaches!", FirstName);
        }

        [Route("update/{FirstName}")]
        [HttpPost()]
        public string updateCoaches(string FirstName, [FromBody] object details)
        {
            var strparse2 = JObject.Parse(details.ToString());
            var LastName = (string)strparse2.SelectToken("LastName");
            var str2 = (string)strparse2.SelectToken("DayOfWork");
            var UserName = (string)strparse2.SelectToken("UserName");
            Boolean Sunday = false;
            Boolean Monday = false;
            Boolean Tuesday = false;
            Boolean Wednesday = false;
            Boolean Thursday = false;
            Boolean Friday = false;
            Boolean Saturday = false;
            int size = str2.Length;
            for (int i = 0; i < size; i++)
                switch (str2[i])
                {
                    case 'a':
                        Sunday = true;
                        break;
                    case 'b':
                        Monday = true;
                        break;
                    case 'c':
                        Tuesday = true;
                        break;
                    case 'd':
                        Wednesday = true;
                        break;
                    case 'e':
                        Thursday = true;
                        break;
                    case 'f':
                        Friday = true;
                        break;
                    case 'g':
                        Saturday = true;
                        break;
                }
            String strPost = "UPDATE Coaches SET FirstName = '" + FirstName + "', LastName = '" + LastName + "',	Sundey = '" + Sunday + "',	Monday = '" + Monday + "',	Tuesday = '" + Tuesday + "',	Wednesday = '" + Wednesday + "',	Thursday = '" + Thursday + "',	Friday = '" + Friday + "',	Saturday = '" + Saturday + "' WHERE UserName = '" + UserName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strPost), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0}'s info updated!", FirstName);
        }

        [Route("all")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<Coaches>> GetAllCoaches()
        {
            string StrAll = "SELECT * FROM Coaches ORDER BY LastName;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<Coaches> list = new List<Coaches>();
            while (sqlDataReader.Read())
            {
                list.Add(new Coaches
                {
                    FirstName = sqlDataReader.GetString(0).ToString().Trim(),
                    LastName = sqlDataReader.GetString(1).ToString().Trim(),
                    UserName = sqlDataReader.GetString(9).ToString().Trim().Remove(sqlDataReader.GetString(9).ToString().Trim().Length - 1)
                });
            }

            sqlConnection.Close();
            return Json(list);
        }

        [Route("{UserName}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<Coaches>> GetByUsername(string UserName)
        {
            string StrAll = "SELECT * FROM Coaches WHERE UserName = '" + UserName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<Coaches> list = new List<Coaches>();
            while (sqlDataReader.Read())
            {
                list.Add(new Coaches
                {
                    FirstName = sqlDataReader.GetString(0).ToString().Trim(),
                    LastName = sqlDataReader.GetString(1).ToString().Trim(),
                    Sunday = sqlDataReader.GetBoolean(2),
                    Monday = sqlDataReader.GetBoolean(3),
                    Tuesday = sqlDataReader.GetBoolean(4),
                    Wednesday = sqlDataReader.GetBoolean(5),
                    Thursday = sqlDataReader.GetBoolean(6),
                    Friday = sqlDataReader.GetBoolean(7),
                    Saturday = sqlDataReader.GetBoolean(8),
                    UserName = sqlDataReader.GetString(9).ToString().Trim().Remove(sqlDataReader.GetString(9).ToString().Trim().Length-1)
                });
            }

            sqlConnection.Close();
            return Json(list);
        }

        [Route("{UserName}")]
        [HttpDelete()]
        public string DeleteCoaches(string UserName)
        {
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(new string[]
            {
                "DELETE FROM Coaches WHERE UserName = '" + UserName + " '; "
            }), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            if (!this._CoachesNames.ContainsKey(UserName))
            {
                return UserName + " deleted.";
            }
            else
                return "UserName not found.";
        }
    }
}
