using Microsoft.EntityFrameworkCore;
using NotesApp.API.Database.Entities;

namespace NotesApp.API.Database.Context
{
    public sealed class NotesAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public NotesAppContext(DbContextOptions<NotesAppContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
