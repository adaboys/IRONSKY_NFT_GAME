using Microsoft.EntityFrameworkCore;

namespace App {
	/// Database management for the app.
	public class AppDbContext : DbContext {
		/// We need this constructor for configuration via `appsetting.json`
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		/// Declare all models for LinQ query translation
		public DbSet<MstAppModel> apps { get; set; }
		public DbSet<UserModel> users { get; set; }
		public DbSet<PlayerModel> players { get; set; }
		public DbSet<UserWalletModel> userWallets { get; set; }

		/// Construct model + Seeding data
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// [Modelizing]
			MstOsModelBuilder.OnModelCreating(modelBuilder);
			MstAppModelBuilder.OnModelCreating(modelBuilder);
			UserModelBuilder.OnModelCreating(modelBuilder);
			PlayerModelBuilder.OnModelCreating(modelBuilder);
			UserWalletModelBuilder.OnModelCreating(modelBuilder);

			// [Seeding]
			MstOsModelBuilder.Seed(modelBuilder);
			MstAppModelBuilder.Seed(modelBuilder);
			UserModelBuilder.Seed(modelBuilder);
		}
	}
}
