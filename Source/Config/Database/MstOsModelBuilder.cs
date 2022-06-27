using Microsoft.EntityFrameworkCore;
using Tool.Compet.Core;

namespace App {
	public class MstOsModelBuilder {
		public static void OnModelCreating(ModelBuilder modelBuilder) {
			// This is auto-increment PK which be configured (annotated with `Key`) at model.
			modelBuilder.Entity<MstOsModel>().Property(model => model.id);
			// Setup default datetime when create
			modelBuilder.Entity<MstOsModel>().Property(model => model.createdAt).HasDefaultValueSql("getdate()");
			// Auto-generate datetime when update
			modelBuilder.Entity<MstOsModel>().Property(model => model.updatedAt).ValueGeneratedOnUpdate();
		}

		public static void Seed(ModelBuilder modelBuilder) {
			modelBuilder.Entity<MstOsModel>().ToTable(DbConst.table_mst_os).HasData(
				new MstOsModel().AlsoDk(model => {
					model.id = 1;
					model.name = "Android";
				}),
				new MstOsModel().AlsoDk(model => {
					model.id = 2;
					model.name = "iOS";
				}),
				new MstOsModel().AlsoDk(model => {
					model.id = 3;
					model.name = "WebGL";
				}),
				new MstOsModel().AlsoDk(model => {
					model.id = 4;
					model.name = "MacOS";
				}),
				new MstOsModel().AlsoDk(model => {
					model.id = 5;
					model.name = "Windows";
				})
			);
		}
	}
}
