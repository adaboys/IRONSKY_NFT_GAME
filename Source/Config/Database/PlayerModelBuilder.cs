using Microsoft.EntityFrameworkCore;

namespace App {
	public class PlayerModelBuilder {
		public static void OnModelCreating(ModelBuilder modelBuilder) {
			// This is auto-increment PK which be configured (annotated with `Key`) at model.
			modelBuilder.Entity<PlayerModel>().Property(model => model.id);
			// Setup default datetime when create
			modelBuilder.Entity<PlayerModel>().Property(model => model.createdAt).HasDefaultValueSql("getdate()");
			// Auto-generate datetime when update
			modelBuilder.Entity<PlayerModel>().Property(model => model.updatedAt).ValueGeneratedOnUpdate();
		}
	}
}
