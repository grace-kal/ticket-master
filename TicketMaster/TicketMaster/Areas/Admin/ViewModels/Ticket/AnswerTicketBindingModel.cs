using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Areas.Admin.ViewModels.Ticket
{
    public class AnswerTicketBindingModel
    {
        public int IdAnsweredTicket { get; set; } //of the answered ticket
        public int IdSendTicket { get; set;  }
        public string AgentId { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SendOn { get; set; }

    }
}
