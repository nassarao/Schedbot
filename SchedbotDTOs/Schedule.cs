using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedbotDTOs
{
    public class Schedule
    {
        /// <summary>
        /// Final = 0, NotFinal = 1, Current = 2, Past = 3 
        /// </summary>
        public enum Flags { Final, NotFinal, Current, Past }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Flags Flag{ get; set; }


        public Schedule(Schedule old)
        {
            StartDate = old.EndDate.AddDays(1);
            EndDate = old.EndDate.AddDays(7);
            Flag = Flags.NotFinal;

        }
        public Schedule()
        {

        }

        public List<Schedule_Shift> PopulateSchedule(List<Shift> shifts)
        {
            List<Schedule_Shift> lss = new List<Schedule_Shift>();
            foreach (Shift shift in shifts)
            {
                Schedule_Shift ss = new Schedule_Shift()
                {
                    ScheduleId = Id,
                    ShiftId = shift.ShiftID
                };
                lss.Add(ss);
            }

            return lss;
        }

    }


    public class Schedule_Shift
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }
        public virtual Schedule Schedule{ get; set; }

        public int ShiftId { get; set; }
        public virtual Shift Shift{ get; set; }
    }
}
