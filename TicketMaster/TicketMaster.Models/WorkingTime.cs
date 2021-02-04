using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicketMaster.Models
{
    public class WorkingTime
    {

        public int Id { get; set; }
        public TimeSpan WorkingSpan { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
