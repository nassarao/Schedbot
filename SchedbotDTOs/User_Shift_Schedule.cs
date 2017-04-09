using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class User_Shift_Schedule
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int ScheduleId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual Schedule Schedule { get; set; }

        public DateTime GetDateTimeOfShift()
        {
            return FindDateFromDateRangeAndDay(Schedule.StartDate, Schedule.EndDate, Shift.Day);
        }


        public DateTime FindDateFromDateRangeAndDay(DateTime Start, DateTime End, DayOfWeek dow)
        {
            for (DateTime date = Start; date.Date <= End.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == dow)
                    return date;
            }
            return new DateTime();
        }
    }
}
