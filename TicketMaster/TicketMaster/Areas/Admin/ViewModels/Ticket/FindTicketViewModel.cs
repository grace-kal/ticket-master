using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.ViewModels.Ticket
{
    public class FindTicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descripton { get; set; }
        public DateTime SendOn { get; set; }
        public Priority Priority { get; set; }
        public string ProjectId { get; set; }
        public string AuthorId { get; set; }
        public string AgentId { get; set; }

        //for the outcoming ticket Id
        public int TicketId { get; set; }

        public bool IsAnswered { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsComplete { get; set; }
    }
}
