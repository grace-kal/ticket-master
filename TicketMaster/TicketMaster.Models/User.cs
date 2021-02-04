using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicketMaster.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            this.Projects = new List<UserProject>();
            this.SendTickets = new List<Ticket>();
            this.AnsweredTickets = new List<Ticket>();
        }

        public bool IsDelete { get; set; }
        public bool IsGlobal { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public IEnumerable<UserProject> Projects { get; set; }

        public IEnumerable<Ticket> SendTickets { get; set; }

        public IEnumerable<Ticket> AnsweredTickets { get; set; }
    }
}
