using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicketMaster.Models
{
   public class Ticket
    {
        public Ticket()
        {
            this.FilesToUpload = new List<File>();
            this.WorkingTimes = new List<WorkingTime>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descripton { get; set; }
        public IEnumerable<File> FilesToUpload { get; set; }
        public DateTime SendOn { get; set; }
        public Priority Priority { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }
        public Project Project { get; set; }

        [ForeignKey("User")]
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsComplete { get; set; }

        public IEnumerable<WorkingTime> WorkingTimes { get; set; }

        [ForeignKey("User")]
        public string AgentId { get; set; }
        public User Agent { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public Ticket ToReplyTicket { get; set; }
    }
}
