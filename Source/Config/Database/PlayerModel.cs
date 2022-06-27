using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// Each user has one or more players in a game.
namespace App {
	[Table(DbConst.table_player)]
	[Index(nameof(userId), nameof(name))]
	public class PlayerModel {
		/// We use unique identifier (uuid, guid) for better data-merging, hard-guessing,...
		[Key]
		[Column("id")]
		public long id { get; set; }

		/// Point to field `user.id`.
		[ForeignKey(DbConst.table_user)]
		[Column("user_id")]
		public Guid userId { get; set; }

		/// Unique player name for visualy differentiate players.
		/// We use `nvarchar` since it may contain some Unicode char.
		[Required]
		[Column("name", TypeName = "nvarchar(256)"), MaxLength(256)]
		public string name { get; set; }

		/// Level in the game.
		[Column("level")]
		public int level { get; set; }

		/// Exp of current level.
		[Column("exp")]
		public float exp { get; set; }

		/// Gold quantity of the player in the game.
		/// This value is for each player, NOT be shared at user's players.
		[Column("gold")]
		public long gold { get; set; }

		[Column("created_at")]
		public DateTime createdAt { get; set; }

		[Column("updated_at")]
		public DateTime? updatedAt { get; set; }

		/// Foreign key property attributes
		public UserModel user { get; set; }
	}
}
