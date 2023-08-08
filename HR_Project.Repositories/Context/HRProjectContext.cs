using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace HR_Project.Repositories.Context
{
    public class HRProjectContext : DbContext
    {
        public HRProjectContext(DbContextOptions<HRProjectContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-AAEQRV0;Database=HR_ProjectDB;Trusted_Connection=True;");
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Advance> Advances { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Key's
            modelBuilder.Entity<User>().HasKey(x => x.ID);
            modelBuilder.Entity<Company>().HasKey(x => x.ID);
            modelBuilder.Entity<Job>().HasKey(x => x.ID);
            modelBuilder.Entity<Department>().HasKey(x => x.ID);
            modelBuilder.Entity<Advance>().HasKey(x => x.ID);

            //Connecttion's
            modelBuilder.Entity<User>().HasOne(x => x.Job).WithMany(x => x.Users).HasForeignKey(x => x.JobID);
            modelBuilder.Entity<User>().HasOne(x => x.Department).WithMany(x => x.Users).HasForeignKey(x => x.DepartmentID);
            modelBuilder.Entity<User>().HasOne(x => x.Company).WithMany(x => x.Users).HasForeignKey(x => x.CompanyID);
            modelBuilder.Entity<Job>().HasOne(x => x.Department).WithMany(x => x.Jobs).HasForeignKey(x => x.DepartmentID);
            modelBuilder.Entity<Department>().HasOne(x => x.Company).WithMany(x => x.Departments).HasForeignKey(x => x.CompanyID);

            modelBuilder.Entity<Advance>().HasOne(x => x.User).WithMany(x => x.Advances).HasForeignKey(x => x.UserID);


            // Seed datas
            modelBuilder.Entity<Company>().HasData(
                new Company { ID = 1, CompanyName = "KardeşlerYazılım", IsActive = true }
            );
            modelBuilder.Entity<Department>().HasData(
                new Department { ID = 1, DepartmentName = "Pazarlama", IsActive = true, CompanyID = 1 }
            );
            modelBuilder.Entity<Job>().HasData(
                new Job { ID = 1, JobName = "Satış Müdürü", IsActive = true, DepartmentID = 1 }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                { ID = 1, FirstName = "Emre", LastName = "Karaüzüm", Password = "123Abc.", Salary = 42000, Gender = Gender.Man, Role = Roles.CompanyManager, PhoneNumber = "5386656649", TCIdentificationNumber = "12345678912", JobID = 1, DepartmentID = 1, CompanyID = 1, IsActive = true, Address = "Ankara Çankaya", Photo = "https://media.licdn.com/dms/image/D4D03AQFKCXDB3b5hSA/profile-displayphoto-shrink_800_800/0/1668897343508?e=2147483647&v=beta&t=FMAUQ8X7dS4I6dL_FgCuWxpxXiwq8hiEIJXeh9K9cEQ", BirthPlace = "Ankara", Birthdate = new DateTime(1991, 1, 1), City=City.ANKARA },
                new User
                {
                    ID = 101,
                    FirstName = "Muhammet",
                    LastName = "Güler",
                    Password = "123Abc.",
                    Salary = 42000,
                    Gender = Gender.Man,
                    City = City.İZMİR,
                    Role = Roles.Admin,
                    Email = "muhammet.guler@bilgeadam.com",
                    PhoneNumber = "5386656649",
                    TCIdentificationNumber = "12345678912",
                    JobID = 1,
                    DepartmentID = 1,
                    CompanyID = 1,
                    IsActive = true,
                    Address = "İzmir",
                    Photo = "https://media.licdn.com/dms/image/D4D03AQHDyrxhHyFbzw/profile-displayphoto-shrink_800_800/0/1685523959801?e=2147483647&v=beta&t=8SZPlG5BfMKyNQmqBIBbRJ2C45EYBsfAaJAf-Rc5oV4",
                    BirthPlace = "İzmir",
                    Birthdate = new DateTime(1991, 1, 1)
                }
            );
        }

    }
}
