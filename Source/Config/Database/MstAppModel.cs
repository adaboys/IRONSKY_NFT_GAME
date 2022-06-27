using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App {
	[Table(DbConst.table_mst_app)]
	public class MstAppModel {
		[Key]
		[Column("id")]
		public int id { get; set; }

		/// Point to PK of table `mst_os`
		[Required]
		[Column("os_id")]
		[ForeignKey(DbConst.table_mst_os)]
		public int osId { get; set; }

		/// For eg,. "2.12.108"
		[Required]
		[Column("version", TypeName = "varchar(256)"), MaxLength(256)]
		public string version { get; set; }

		[Required]
		[Column("created_at", TypeName = "datetime")]
		public DateTime createdAt { get; set; }

		[Column("updated_at", TypeName = "datetime")]
		public DateTime? updatedAt { get; set; }

		/// For soft-delete. Note that: avoid physical deletion.
		[Column("deleted_at")]
		public DateTime? deletedAt { get; set; }

		/// Foreign key property attributes
		public MstOsModel os { get; set; }
	}
}
