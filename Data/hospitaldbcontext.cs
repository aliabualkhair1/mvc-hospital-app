using Hospital_Project.Entities;
using Hospital_Project.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Project.Data
{
    public class hospitaldbcontext :IdentityDbContext<UserInfo>
    {
        public hospitaldbcontext(DbContextOptions<hospitaldbcontext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DoctorsTimeTable>().Property(d => d.AvailableTimes).HasConversion<string>();
            modelBuilder.Entity<Doctors>().Property(d=>d.DoctorRank).HasConversion<string>();
            modelBuilder.Entity<DocNurse>().HasKey(ck => new { ck.DoctorId, ck.NurseId });
            modelBuilder.Entity<Nurses>()
       .Property(n => n.Salary)
       .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Employees>()
       .Property(n => n.Salary)
       .HasColumnType("decimal(18,2)"); modelBuilder.Entity<Doctors>()
        .Property(n => n.Salary)
        .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Doctors>()
       .Property(n => n.BookingSalary)
       .HasColumnType("decimal(18,2)");

        }
        public DbSet <Appointments> appointments { get; set; }
        public DbSet<Doctors> doctors { get; set; }
        public DbSet <Patients> patients { get; set; }
        public DbSet <Employees> employees { get; set; }
        public DbSet <Department> departments { get; set; }
        public DbSet <Nurses> nurses { get; set; }
        public DbSet <DoctorsTimeTable> DoctorAvailableDate { get; set; }
        public DbSet <ComplaintsAndSuggestions> ComplaintsAndSuggestions { get; set; }
        public DbSet <DocNurse> DocNurse { get; set; }
    }
}
