using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// MS SQL data types:
/// https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver15
/// Below is some data types, note that, `ByteCount <= 256` when storing string
/// will better performance since they are stored in memory instead of disk storage.
/// - char: fixed length characters in UTF-8., it take 1 byte per character.
/// - varchar: various length characters in UTF-8, it take 1 byte per character.
/// - nchar: fixed length characters in Unicode (UTF-16?), it take 2 bytes per character.
/// - nvarchar: various length characters in Unicode (UTF-16?), it take 2 bytes per character.

/// [C# Data Type] =>  [SQL Server Data Type]
/// int            =>  int
/// string         =>  nvarchar(Max)
/// decimal        =>  decimal(18,2)
/// float          =>  real
/// byte[]         =>  varbinary(Max)
/// datetime       =>  datetime
/// bool           =>  bit
/// byte           =>  tinyint
/// short          =>  smallint
/// long           =>  bigint
/// double         =>  float
/// char           =>  No mapping
/// sbyte          =>  No mapping (throws exception)
/// object         =>  No mapping

namespace App {
	[Table(DbConst.table_user)]
	[Index(nameof(email), IsUnique = true)]
	[Index(nameof(code), IsUnique = true)]
	public class UserModel {
		/// We use unique identifier (uuid, guid) for better data-merging, id-confliction, hard-guessing,...
		/// Use `Key` attribute to mark this as PK.
		[Key]
		[Column("id")]
		public Guid id { get; set; }

		/// Unique email that can be used for login
		[Column("email", TypeName = "varchar(256)"), MaxLength(256)]
		public string? email { get; set; }

		/// Unique nickname that can be used for login (see `email`)
		[Column("code", TypeName = "varchar(256)"), MaxLength(256)]
		public string? code { get; set; }

		/// Encrypted password
		[Column("password", TypeName = "varchar(256)"), MaxLength(256)]
		public string? password { get; set; }

		/// Sex type
		[Column("gender")]
		public byte gender { get; set; }

		/// Which level of privilege that user have.
		/// Higher value indicates higher privilege.
		[Column("role")]
		public byte role { get; set; } = UserTableConst.ROLE_USER;

		/// Gem quantity of the user. This value will be shared at this user's players,
		/// and this is currency of the game (have ratio with real money).
		/// For eg, 100 gem = real 1 dollar.
		[Column("gem")]
		public long gem { get; set; }

		/// General status for use permission. See `UserConst.STATUS_*`.
		[Column("status")]
		public byte status { get; set; } = UserTableConst.STATUS_NORMAL;

		/// Ref: https://english.stackexchange.com/questions/104740/created-at-or-created-in
		/// - Use `created at` for precise times.
		/// - Use `created on` for days, dates.
		/// - Use `created in` for duration.
		[Column("created_at")]
		public DateTime createdAt { get; set; }

		[Column("updated_at")]
		public DateTime? updatedAt { get; set; }

		/// Foreign key property attributes
		public ICollection<PlayerModel> players { get; set; }
	}
}
