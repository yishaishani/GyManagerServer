using System;

namespace GyManagerAPI.Controllers
{
    public class Trainers
    {
        public string FirstName;

        public string LastName;

        public string Gender;

        public string PhoneNumber;

        public string Email;

        public override string ToString()
        {
            return this.FirstName + ":" + this.LastName + ":" + this.Gender + ":" + this.PhoneNumber + ":" + this.Email + ":" + "\n\r";
        }
    }
}
