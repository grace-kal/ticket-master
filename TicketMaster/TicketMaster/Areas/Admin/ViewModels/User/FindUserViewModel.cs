using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Areas.Admin.ViewModels.User
{
    public class FindUserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public bool IsGlobal { get; set; }

    }
}
