using Microsoft.EntityFrameworkCore;
using Task10Field.Models;
using Task10Field.Models.Binding;
using Task10Field.Models.Payment;
using Task10Field.Models.Verification;

namespace Task10Field.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<StudentData> StudentData { get; set; }
        public DbSet<OTPVerification> OTPVerifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Task10Field.Models.InnerViewModel> InnerViewModel { get; set; } = default!;
    }
}
