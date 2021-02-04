using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.ViewModels.Project
{
    public class FindProjectViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<UserProject> Workers { get; set; }
        public IEnumerable<Models.Ticket> IncomingTickets { get; set; }
        public bool IsDelete { get; set; }
        public TimeSpan WorkTime { get; set; }
    }
}
