using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Models
{
    public class EditTicketBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public DateTime SendOn { get; set; }
        public IEnumerable<IFormFile> FilesToUpload { get; set; }
        //public string AuthorId { get; set; }
    }
}
