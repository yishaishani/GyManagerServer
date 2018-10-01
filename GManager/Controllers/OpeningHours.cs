namespace GyManagerAPI.Controllers
{
    public class OpeningHours
    {
        public string DAY;

        public string HOURS;

        public override string ToString()
        {
            return this.DAY + ":" + this.HOURS + "\n\r";
        }
    }
}
