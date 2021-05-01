using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlayList.Models
{
    public class AppDB: DbContext
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options)
        {

        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<PlayListVideo> PlayListVideos { get; set; }
    }
}
