using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Task3
{
    public class mydata
    {
        public int Id { get; set; }
        public byte[] data { get; set; }
    }
    public class myFace
    {
        public int Id { get; set; }
        public int Hash { get; set; }
        public byte[] Embedding { get; set; }
        public mydata myImage { get; set; }

    }
    class LibraryContext : DbContext
    {
        public DbSet<mydata> mydatas { get; set; }
        public DbSet<myFace> myFaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder o)
            => o.UseSqlite("Data Source=library.db");
    }

}

