using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App {
	public class RegisterPlayerRequestBody {
		[Required]
		[JsonPropertyName(name: "player_name")]
		public string playerName { get; set; }
	}

	public class RegisterPlayerResponse : ApiSuccessResponse {
		public const int player_name_duplicated = -1;

		[JsonPropertyName(name: "data")]
		public RegisterPlayerResponseData data { get; set; }

		public class RegisterPlayerResponseData {
			[JsonPropertyName(name: "player_id")]
			public long playerId { get; set; }

			[JsonPropertyName(name: "player_name")]
			public string playerName { get; set; }
		}
	}

	public class ChangePlayerNameRequestBody {
		[Required]
		[JsonPropertyName(name: "player_id")]
		public long playerId { get; set; }

		[Required]
		[JsonPropertyName(name: "new_player_name")]
		public string newPlayerName { get; set; }
	}
}
