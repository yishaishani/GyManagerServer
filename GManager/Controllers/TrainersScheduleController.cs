using GyManagerAPI.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GManager.Controllers
{
    [RoutePrefix("TrainersSchedule")]
    public class TrainersScheduleController : ApiController
    {
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";
        private Dictionary<string, string> _TrainersSchedule = new Dictionary<string, string>();

        [Route("add/{TrainersUserName}")]
        [HttpPost()]
        public string AddTrainersClass(string TrainersUserName, [FromBody]object Times)
        {

            string slotsCalc = "";
            string Insert = "insert into TrainersSchedule values ";
            var strparse = JObject.Parse(Times.ToString());
            var UserName = (string)strparse.SelectToken("UserName");
            int ID = (int)strparse.SelectToken("ID");
            DateTime start = DateTime.Parse((string)strparse.SelectToken("StartTime"));
            DateTime end = DateTime.Parse((string)strparse.SelectToken("EndTime"));
            TimeSpan ts = end - start;
            int slots = (int)ts.TotalMinutes / 30;

            for (int i = 0; i < slots; i++, start = start.AddMinutes(30))
            {
                slotsCalc = string.Format("('{0}','{1}','{2}','{3}','{4}')", TrainersUserName, start.ToString("yyyy-MM-dd HH:mm:ss"), start.AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss"), ID , UserName);
                if (i == slots - 1)
                    slotsCalc += ";";
                else
                    slotsCalc += ",";
                Insert += slotsCalc;
            }

            Insert.Replace('/', '-');
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(Insert), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("Data Entered!");
        }

        [Route("all/{UserName}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<TrainersSchedule>> GetAllClasses(string UserName)
        {
            String StrAll = "SELECT * FROM TrainersSchedule WHERE UserName = '" + UserName + "' ;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<TrainersSchedule> list = new List<TrainersSchedule>();
            while (sqlDataReader.Read())
            {
                list.Add(new TrainersSchedule
                {
                    UserName = sqlDataReader.GetString(0).ToString().Trim().Remove(sqlDataReader.GetString(0).ToString().Trim().Length - 1),
                    StartTime = sqlDataReader.GetDateTime(1),
                    EndTime = sqlDataReader.GetDateTime(2),
                    ID = sqlDataReader.GetInt32(3),
                    CoachesUserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }

            sqlConnection.Close();

            return Json(list);
        }

        [Route("allTrainers/{UserName}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<TrainersSchedule>> AllTrainers(string UserName)
        {
            String StrAll = "SELECT * FROM TrainersSchedule WHERE UserName = '" + UserName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<TrainersSchedule> list = new List<TrainersSchedule>();
            while (sqlDataReader.Read())
            {
                list.Add(new TrainersSchedule
                {
                    UserName = sqlDataReader.GetString(0).ToString().Trim().Remove(sqlDataReader.GetString(0).ToString().Trim().Length - 1),
                    StartTime = sqlDataReader.GetDateTime(1),
                    EndTime = sqlDataReader.GetDateTime(2),
                    ID = sqlDataReader.GetInt32(3),
                    CoachesUserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }

            sqlConnection.Close();

            return Json(list);
        }

        [Route("{ID}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<TrainersSchedule>> ID(string ID)
        {
            String StrAll = "SELECT * FROM TrainersSchedule WHERE ID = '" + ID + "' ;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<TrainersSchedule> list = new List<TrainersSchedule>();
            while (sqlDataReader.Read())
            {
                list.Add(new TrainersSchedule
                {
                    UserName = sqlDataReader.GetString(0).ToString().Trim().Remove(sqlDataReader.GetString(0).ToString().Trim().Length - 1),
                    StartTime = sqlDataReader.GetDateTime(1),
                    EndTime = sqlDataReader.GetDateTime(2),
                    ID = sqlDataReader.GetInt32(3),
                    CoachesUserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }

            sqlConnection.Close();

            return Json(list);
        }

        [Route("DeleteClass/{ID}")]
        [HttpDelete()]
        public string DeleteTrainersclass(string ID)
        {
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(new string[]
            {
                "DELETE FROM TrainersSchedule WHERE ID = '" + ID + " '; "
            }), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            if (!this._TrainersSchedule.ContainsKey(ID))
            {
                return ID + " deleted.";
            }
            else
                return "UserName not found.";
        }
    }
}
