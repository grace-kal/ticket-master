using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Areas.Admin.ViewModels.Ticket
{
    public class DisplayAllTicketsViewModel
    {
       public List<TicketMaster.Models.Ticket> AllTickets { get; set; }
        public string CurrentUserId { get; set; }//because  i want to create ticket with definite author id that equals the current logged user
    }
}
