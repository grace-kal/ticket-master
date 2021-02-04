using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Data;

namespace TicketMaster.Seeder
{
    public interface ISeeder
    {
        Task SeedAsync(TicketMasterDbContext dbContext, IServiceProvider serviceProvider);
    }
}
