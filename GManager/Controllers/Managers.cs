using System;

namespace GyManagerAPI.Controllers
{
    public class Manager
    {
        public string FirstName;

        public string LastName;

        public string PhoneNumber;

        public string Email;

        public string UserName;

        public override string ToString()
        {
            return this.FirstName + ":" + this.LastName +  ":" + this.PhoneNumber + ":" + this.Email + ":" + this.UserName + "\n\r";
        }
    }
}
