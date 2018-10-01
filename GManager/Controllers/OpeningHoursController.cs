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
using System.Web.Http.Results;

namespace GManager.Controllers
{
    [RoutePrefix("OpeningHours")]
    public class OpeningHoursController : ApiController
    {
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";
        private Dictionary<string, string> _OpeningHours = new Dictionary<string, string>();
    
        [Route("all")]
        [HttpGet()]
        public System.Web.Http.Results.JsonResult<List<OpeningHours>> GetAllHours()
        {
            string StrAll = "SELECT * FROM OpeningHours;";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand(StrAll, sqlConnection).ExecuteReader();
            List<OpeningHours> list = new List<OpeningHours>();
            while (sqlDataReader.Read())
            {
                list.Add(new OpeningHours
                {
                    DAY = sqlDataReader.GetString(0).ToString().Trim(),
                    HOURS = sqlDataReader.GetString(1).ToString().Trim()
                });
            }
            
            sqlConnection.Close();
           
            return Json(list);
        }
        
        [Route("{update}")]
        [HttpPost()]
        public string update(string update, [FromBody]object HOURS)
        {

            var strparse2 = JObject.Parse(HOURS.ToString());
            var str1 = (string)strparse2.SelectToken("Sunday");

            var str2 = (string)strparse2.SelectToken("Monday");
            
            var str3 = (string)strparse2.SelectToken("Tuesday");
            
            var str4 = (string)strparse2.SelectToken("Wednesday");
            
            var str5 = (string)strparse2.SelectToken("Thursday");
            
            var str6 = (string)strparse2.SelectToken("Friday");
            
            var str7 = (string)strparse2.SelectToken("Saturday");
            String strGetNew = update +" OpeningHours SET HOURS = CASE DAY WHEN 'Sunday' THEN '" + str1 + "' WHEN 'Monday' THEN '" + str2 + "' WHEN 'Tuesday' THEN '" + str3 + "' WHEN 'Wednesday' THEN '" + str4 + "' WHEN 'Thursday' THEN '" + str5 + "' WHEN 'Friday' THEN '" + str6 + "' WHEN 'Saturday' THEN '" + str7 + "' ELSE HOURS END WHERE DAY IN('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'); ";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strGetNew), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("opening hours is update!");
        }

    }
}
