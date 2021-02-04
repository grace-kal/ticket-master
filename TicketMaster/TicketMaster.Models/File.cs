using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicketMaster.Models
{
    public class File
    {

        public int Id { get; set; }
        public string Alt { get; set; }
        public byte[] FileUpload { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

    }
}
