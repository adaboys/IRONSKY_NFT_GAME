using Microsoft.EntityFrameworkCore;
using Tool.Compet.Core;

namespace App {
	public class MstAppModelBuilder {
		public static void OnModelCreating(ModelBuilder modelBuilder) {
			// This is auto-increment PK which be configured (annotated with `Key`) at model.
			modelBuilder.Entity<MstAppModel>().Property(model => model.id);
			// Setup default datetime when create
			modelBuilder.Entity<MstAppModel>().Property(model => model.createdAt).HasDefaultValueSql("getdate()");
			// Auto-generate datetime when update
			modelBuilder.Entity<MstAppModel>().Property(model => model.updatedAt).ValueGeneratedOnUpdate();
		}

		public static void Seed(ModelBuilder modelBuilder) {
			modelBuilder.Entity<MstAppModel>().ToTable(DbConst.table_mst_app).HasData(
				new MstAppModel().AlsoDk(model => {
					model.id = 1;
					model.osId = MstOsTableConst.ID_ANDROID;
					model.version = "1.0.0";
				}),
				new MstAppModel().AlsoDk(model => {
					model.id = 2;
					model.osId = MstOsTableConst.ID_IOS;
					model.version = "1.0.0";
				})
			);
		}
	}
}
