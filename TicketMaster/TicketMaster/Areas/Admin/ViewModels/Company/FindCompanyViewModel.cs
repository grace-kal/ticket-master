using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.ViewModels.Company
{
    public class FindCompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Models.User> Workers { get; set; }
        public IEnumerable<Models.Project> Projects { get; set; }
        public bool IsDeleted { get; set; }
    }
}
