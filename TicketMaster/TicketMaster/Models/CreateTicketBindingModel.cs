using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Models
{
    public class CreateTicketBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public string ProjectId { get; set; }
        public DateTime SendOn { get; set; }
        public string AuthorId { get; set; }
        public string AgentId { get; set; }
    }
}
