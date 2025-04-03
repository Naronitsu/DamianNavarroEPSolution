using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PollDbContext : DbContext
    {
        public PollDbContext(DbContextOptions<PollDbContext> options) : base(options) { }

        public DbSet<Poll> Polls { get; set; }
    }
}