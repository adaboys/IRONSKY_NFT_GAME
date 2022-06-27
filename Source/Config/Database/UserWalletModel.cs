using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App {
	[Table(DbConst.table_user_wallet)]
	[Index(nameof(internalWalletAddress), nameof(externalWalletAddress))]
	public class UserWalletModel {
		// PK
		[Key]
		[Column("id")]
		public long id { get; set; }

		/// Point to field `user.id`.
		[Required]
		[ForeignKey(DbConst.table_user)]
		[Column("user_id")]
		public Guid userId { get; set; }

		/// Wallet address of IronSky itself
		[Required]
		[Column("internal_wallet_address", TypeName = "varchar(256)"), MaxLength(256)]
		public string internalWalletAddress { get; set; }

		/// Wallet address of 3rd-party (Nami, Yoroi,...)
		[Required]
		[Column("external_wallet_address", TypeName = "varchar(256)"), MaxLength(256)]
		public string externalWalletAddress { get; set; }

		[Column("created_at")]
		public DateTime createdAt { get; set; }

		[Column("updated_at")]
		public DateTime? updatedAt { get; set; }

		[Column("deleted_at")]
		public DateTime? deletedAt { get; set; }

		/// Foreign key property attributes
		public UserModel user { get; set; }
	}
}
