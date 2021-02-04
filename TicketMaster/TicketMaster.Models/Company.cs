using System;
using System.Collections.Generic;
using System.Text;

namespace TicketMaster.Models
{
    public class Company
    {
        public Company()
        {
            this.Workers = new List<User>();
            this.Projects = new List<Project>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Workers { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public bool IsDeleted { get; set; }

    }
}
