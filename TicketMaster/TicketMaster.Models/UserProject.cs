using System;
using System.Collections.Generic;
using System.Text;

namespace TicketMaster.Models
{
   public class UserProject
    {

        public string UserId { get; set; }
        public User Worker { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
