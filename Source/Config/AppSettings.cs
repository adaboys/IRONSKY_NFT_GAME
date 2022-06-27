/// This mapping with `appsettings.json` file to avoid
/// hardcoded setting-reference at multiple places.

/// dkopt: For better we should annotate fields to each name if appsettings.json
/// to avoid changes in refactoring process.

namespace App {
	public class AppSetting {
		public ConnectionStrings connectionStrings { get; set; }
		public JwtSetting jwt { get; set; }
		public CardanoNode cardanoNode { get; set; }

		public class ConnectionStrings {
			public string itself { get; set; }
		}

		public class JwtSetting {
			public string key { get; set; }
			public string issuer { get; set; }
			public string audience { get; set; }
			public string subject { get; set; }
		}

		public class CardanoNode {
			public string apiBaseUrl { get; set; }
		}
	}
}
