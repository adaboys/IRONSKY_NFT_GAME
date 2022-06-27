
namespace App {
	public class DbConst {
		/// OS info (Android, iOS, WebGL,...)
		public const string table_mst_os = "mst_os";

		/// App info (version, name,...).
		public const string table_mst_app = "mst_app";

		/// In future, this table maybe be moved to core-server.
		/// So we should NOT put specific fields of this game to this table.
		public const string table_user = "user";

		/// Player info for the game.
		/// Note that, each user has multiple players account, each player can hold multiple characters.
		public const string table_player = "player";

		/// Stores wallets (Nami, Yoroi,...) of each user.
		public const string table_user_wallet = "user_wallet";
	}
}
