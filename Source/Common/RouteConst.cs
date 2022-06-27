/// Api routes from this api server.
namespace App {
	public class RouteConst {
		public const string api_prefix = "api";

		/// [App]
		public const string app_info = "app/{os_id}/info";

		/// [Auth]
		public const string auth_verify = "auth/verify";
		public const string auth_login = "auth/login";
		public const string auth_provider_login = "auth/provider/login";
		public const string auth_logout = "auth/logout";

		/// [User]
		public const string user_createWithExternalWallet = "user/createWithExternalWallet";

		/// [Player]
		public const string player_verify = "player/{player_id}/verify";
		public const string player_register = "player/register";
		public const string player_name_change = "player/name/change";
		public const string player_data_all = "player/{player_id}/data/all";

		/// Api routes from Cardano node server.
		public class CardanoNode {
			/// [Wallet]
			public const string wallet_create = "wallet/create";
		}
	}
}
