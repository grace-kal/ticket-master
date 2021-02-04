using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Areas.Admin.ViewModels.Project
{
    public class DisplayAllProjectIncomingTicketsViewModel
    {
        public List<TicketMaster.Models.Ticket> IncomingTickets { get; set; }
    }
}
