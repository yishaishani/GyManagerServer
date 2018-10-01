using System;

namespace GyManagerAPI.Controllers
{
    public class Coaches
    {
        public string FirstName;

        public string LastName;

        public bool Sunday;

        public bool Monday;

        public bool Tuesday;

        public bool Wednesday;

        public bool Thursday;

        public bool Friday;

        public bool Saturday;

        public string UserName;

        public override string ToString()
        {
            return this.FirstName + ":" + this.LastName + ":" + this.Sunday + ":" + this.Monday + ":" + this.Tuesday + ":" + this.Wednesday + ":" + this.Thursday + ":" + this.Friday + ":" + this.Saturday + ":" + this.UserName + "\n\r";
        }
    }
}
