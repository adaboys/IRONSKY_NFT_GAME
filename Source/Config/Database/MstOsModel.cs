using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App {
	[Table(DbConst.table_mst_os)]
	public class MstOsModel {
		[Key]
		[Column("id")]
		public int id { get; set; }

		/// For eg,. "Android", "iOS", "WebGL", "MacOS", "Windows",...
		[Required]
		[Column("name", TypeName = "varchar(256)"), MaxLength(256)]
		public string name { get; set; }

		[Column("created_at")]
		public DateTime createdAt { get; set; }

		[Column("updated_at")]
		public DateTime? updatedAt { get; set; }

		/// For soft-delete. Note that: avoid physical deletion.
		[Column("deleted_at")]
		public DateTime? deletedAt { get; set; }
	}
}
