using System;

namespace GyManagerAPI.Controllers
{
    public class ScheduleCoaches
    {
        
        public string UserName;

        public string StartTime;

        public string EndTime;

        public int ID;

        public string TranierUserName;

        public override string ToString()
        {
            return this.UserName + ":" + this.StartTime + ":" + this.EndTime + ":" + this.ID + ":" + this.TranierUserName + "\n\r";
        }
    }
}
