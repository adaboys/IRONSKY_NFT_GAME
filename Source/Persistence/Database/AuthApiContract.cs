using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App {
	public class VerifyAuthRequestBody {
		[Required]
		[JsonPropertyName(name: "access_token")]
		public string accessToken { get; set; }
	}

	public class VerifyAuthResponse : ApiSuccessResponse {
		[JsonPropertyName(name: "data")]
		public VerifyAuthResponseData data { get; set; }

		public class VerifyAuthResponseData {
			[JsonPropertyName(name: "user_id")]
			public string userId { get; set; }
		}
	}

	public class LoginRequestBody {
		[Required]
		[JsonPropertyName(name: "id")]
		public string id { get; set; }

		[Required]
		[JsonPropertyName(name: "password")]
		public string password { get; set; }
	}

	public class LoginResponse : ApiSuccessResponse {
		[JsonPropertyName(name: "data")]
		public LoginResponseData data { get; set; }

		public class LoginResponseData {
			[JsonPropertyName(name: "access_token")]
			public string accessToken { get; set; }
		}
	}

	public class ProviderLoginRequestBody {
		[Required]
		[JsonPropertyName(name: "provider")]
		public string provider { get; set; }

		[Required]
		[JsonPropertyName(name: "access_token")]
		public string accessToken { get; set; }
	}

	public class LogoutRequestBody {
		[JsonPropertyName(name: "log_out_at_every_where")]
		public bool logOutEveryWhere { get; set; }
	}
}
