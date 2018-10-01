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
    [RoutePrefix("CoachesSchedule")]
    public class ScheduleCoachesController : ApiController
    {
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";
        private Dictionary<string, string> _CoachesSchedule = new Dictionary<string, string>();

        [Route("{add}")]
        [HttpPost()]
        public string AddCoachesClasses( [FromBody]object Times)
        {
            
            string slotsCalc = "";
            string Insert = "insert into CoachesSchedule values ";
            var strparse = JObject.Parse(Times.ToString());
            var UserName = (string)strparse.SelectToken("UserName");
            DateTime start = DateTime.Parse((string)strparse.SelectToken("StartTime"));
            DateTime end = DateTime.Parse((string)strparse.SelectToken("EndTime"));
            TimeSpan ts = end - start;
            int slots = (int)ts.TotalMinutes / 30;

            for (int i = 0; i < slots; i++, start = start.AddMinutes(30))
            {
                slotsCalc = string.Format("('{0}','{1}','{2}','empty')", UserName , start.ToString("yyyy-MM-dd HH:mm:ss") , start.AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss"));
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
        public System.Web.Http.Results.JsonResult<List<ScheduleCoaches>> GetAllClasses(string UserName)
        {
            String StrAll = "SELECT * FROM CoachesSchedule WHERE UserName = '"+ UserName + "' ;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<ScheduleCoaches> list = new List<ScheduleCoaches>();
            while (sqlDataReader.Read())
            {
                list.Add(new ScheduleCoaches
                {
                    UserName = sqlDataReader.GetString(0).ToString().Trim().Remove(sqlDataReader.GetString(0).ToString().Trim().Length - 1),
                    StartTime = sqlDataReader.GetDateTime(1).ToString("HH:mm dd/MM/yyyy"),
                    EndTime = sqlDataReader.GetDateTime(2).ToString("HH:mm dd/MM/yyyy"),
                    ID = sqlDataReader.GetInt32(3),
                    TranierUserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }
 
            sqlConnection.Close();
            
            return Json(list);
        }

        [Route("allTrainers/{TranierUserName}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<ScheduleCoaches>> GetAllTrainersClasses(string TranierUserName)
        {
            String StrAll = "SELECT * FROM CoachesSchedule WHERE TranierUserName = '" + TranierUserName + "';";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<ScheduleCoaches> list = new List<ScheduleCoaches>();
            while (sqlDataReader.Read())
            {
                list.Add(new ScheduleCoaches
                {
                    UserName = sqlDataReader.GetString(0).ToString().Trim().Remove(sqlDataReader.GetString(0).ToString().Trim().Length - 1),
                    StartTime = sqlDataReader.GetDateTime(1).ToString("HH:mm dd/MM/yyyy"),
                    EndTime = sqlDataReader.GetDateTime(2).ToString("HH:mm dd/MM/yyyy"),
                    ID = sqlDataReader.GetInt32(3),
                    TranierUserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }

            sqlConnection.Close();

            return Json(list);
        }

        [Route("ID/{ID}")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<ScheduleCoaches>> GetByID(string ID)
        {
            
            String StrAll = "SELECT * FROM CoachesSchedule WHERE ID = '" + ID + "' ;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<ScheduleCoaches> list = new List<ScheduleCoaches>();
            while (sqlDataReader.Read())
            {
                list.Add(new ScheduleCoaches
                {
                    UserName = sqlDataReader.GetString(0).ToString().Trim().Remove(sqlDataReader.GetString(0).ToString().Trim().Length - 1),
                    StartTime = sqlDataReader.GetDateTime(1).ToString("HH:mm dd/MM/yyyy"),
                    EndTime = sqlDataReader.GetDateTime(2).ToString("HH:mm dd/MM/yyyy"),
                    ID = sqlDataReader.GetInt32(3),
                    TranierUserName = sqlDataReader.GetString(4).ToString().Trim().Remove(sqlDataReader.GetString(4).ToString().Trim().Length - 1)
                });
            }


            sqlConnection.Close();

            return Json(list);
        }

        [Route("UpdateID")]
        [HttpPost()]
        public string UpdateID([FromBody]object Details)
        {
            var strparse = JObject.Parse(Details.ToString());
            var TrainersUserName = (string)strparse.SelectToken("TrainersUserName");
            var ID = (int)strparse.SelectToken("ID");
            string StrSql = string.Format("update CoachesSchedule set TranierUserName = '{0}' where ID = '{1}';",TrainersUserName,ID);
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(StrSql), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("Data Updated!");
        }

        [Route("DeleteClass/{ID}")]
        [HttpDelete()]
        public string DeleteClass(string ID)
        {
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(new string[]
            {
                "DELETE FROM CoachesSchedule WHERE ID = '" + ID + " '; "
            }), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            if (!this._CoachesSchedule.ContainsKey(ID))
            {
                return ID + " deleted.";
            }
            else
                return "UserName not found.";
        }
    }
}
