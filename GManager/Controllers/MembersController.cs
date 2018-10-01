using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GyManagerAPI.Controllers
{
    [RoutePrefix("Members")]
    public class MembersController : ApiController
    {
        private Dictionary<string, string> _userName = new Dictionary<string, string>();
        public String strCon = "Data Source=SQL5018.SmarterASP.NET;Initial Catalog=DB_A26C48_elyi;User Id=DB_A26C48_elyi_admin;Password=shani1989;";
       
        [Route("{userName}")]
        [HttpPost()]
        public string AddMember(string userName, [FromBody] object Password)
        {
            var obj = JObject.Parse(Password.ToString());
            var url = (string)obj.SelectToken("Password");
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(url.ToString()));
            String strPost = "INSERT INTO Members VALUES('" + userName + "' , '" + System.Text.Encoding.ASCII.GetString(sha1data) + "')";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            DbCommand commend = new SqlCommand(string.Concat(strPost), sqlConnection);
            sqlConnection.Open();
            commend.ExecuteNonQuery();
            sqlConnection.Close();
            return string.Format("{0} added to the members!", userName);
        }

        [Route("{userName}/{password}")]
        [HttpGet()]
        public Boolean verify(string userName, string Password)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Password));
            String StrGetPass = "SELECT Password FROM Members WHERE UserName = '" + userName + "'";
            SqlConnection sqlConnection = new SqlConnection(strCon);
            sqlConnection.Open();
            SqlCommand sqlDataReader = new SqlCommand(StrGetPass, sqlConnection);
            string pass = (string)sqlDataReader.ExecuteScalar();
            if (pass == System.Text.Encoding.ASCII.GetString(sha1data))
            {
                sqlConnection.Close();
                return true;
            }

            else
            {
                sqlConnection.Close();
                return false;
            }
            
        }

    }
}

