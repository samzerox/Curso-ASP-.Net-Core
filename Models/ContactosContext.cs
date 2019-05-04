using Microsoft.EntityFrameworkCore;

namespace contactos.Models
{
    public class ContactosContext: DbContext
    {
        public ContactosContext(DbContextOptions<ContactosContext> options)
            : base(options)
            {

            }
            public DbSet<Contacto> Contacto {get; set;}
            public DbSet<Usuario> Usuario {get; set;}
    }
}