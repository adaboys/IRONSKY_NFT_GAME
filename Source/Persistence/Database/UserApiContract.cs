using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App {
	public class CreateUserRequestBody {
		[Required]
		[JsonPropertyName(name: "wallet_name")]
		public string externalWalletName { get; set; }

		[Required]
		[JsonPropertyName(name: "wallet_address")]
		public string externalWalletAddress { get; set; }
	}

	public class CreateUserResponse : ApiSuccessResponse {
		[JsonPropertyName(name: "data")]
		public CreateUserData data { get; set; }

		public class CreateUserData {
			[JsonPropertyName(name: "internal_wallet_address")]
			public string internalWalletAddress { get; set; }
		}
	}
}
