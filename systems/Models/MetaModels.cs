using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace systems.Models
{
    public class FullReservation
    {
        public BasicReservation basic { get; set; }
        public ExtendedReservation extended { get; set; }

        public FullReservation()
        {
            basic = new BasicReservation();
            extended = new ExtendedReservation();
        }

        //public FullReservation(int id)
        //{
        //    RoomResEntities db = new RoomResEntities();
        //    basic = db.BasicReservations.Single(m => m.ResId == id);
        //    extended = basic.ExtendedReservation;
        //}
    }
}