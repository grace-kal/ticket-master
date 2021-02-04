using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.ViewModels.User
{
    public class DisplayAllUserSendTicketsViewModel
    {
        public List<Models.Ticket> SendTickets { get; set; }
    }
}
