using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeuroTumAI.Core.Entities.Admin;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Contact_Us;
using NeuroTumAI.Core.Entities.Post_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Repository.Data
{
	public class StoreContext : IdentityDbContext<ApplicationUser>
	{
		public StoreContext(DbContextOptions<StoreContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}

		//public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Admin> Admins { get; set; }
	}
}
