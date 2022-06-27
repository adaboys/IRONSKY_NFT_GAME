
namespace App {
	using System.Text.Json.Serialization;

	public class CreateInternalWalletResponse : ApiSuccessResponse {
		[JsonPropertyName(name: "data")]
		public Data data { get; set; }

		public class Data {
			[JsonPropertyName(name: "wallet_address")]
			public string internalWalletAddress { get; set; }
		}
	}
}
