using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicketMaster.Models
{
 
        public class Project
        {
            public Project()
            {
                this.Workers = new List<UserProject>();
                this.IncomingTickets = new List<Ticket>();
            }

            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
         
            [ForeignKey("Company")]
            public int CompanyId { get; set; }
            public Company Company { get; set; }

            public IEnumerable<UserProject> Workers { get; set; }
            public bool IsDeleted { get; set; }
            public TimeSpan WorkTime { get; set; }
            public IEnumerable<Ticket> IncomingTickets { get; set; }



        }
}
