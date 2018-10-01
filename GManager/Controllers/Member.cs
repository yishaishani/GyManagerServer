using System;

namespace GyManagerAPI.Controllers
{
	internal class Member
	{
		public string userName;

		public string password;

		public override string ToString()
		{
			return this.userName + ":" + this.password + "\n\r";
		}
	}
}
