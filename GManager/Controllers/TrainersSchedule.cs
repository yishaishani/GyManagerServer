using System;

namespace GyManagerAPI.Controllers
{
    public class TrainersSchedule
    {

        public string UserName;

        public DateTime StartTime;

        public DateTime EndTime;

        public int ID;

        public string CoachesUserName;

        public override string ToString()
        {
            return this.UserName + ":" + this.StartTime + ":" + this.EndTime + ":" + this.ID + ":" + this.CoachesUserName + "\n\r";
        }
    }
}
