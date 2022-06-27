using Tool.Compet.Core;

namespace App {
	// Info of current environment.
	public class Env {
		public const string DEVELOPMENT = "Development";
		public const string STAGING = "Staging";
		public const string PRODUCTION = "Production";

		/// Get name of current environment.
		/// Check returned value with this class's constants.
		/// Technically, env name is obtained from `environmentVariables:ASPNETCORE_ENVIRONMENT`
		/// in file `./Properties/launchSettings.json`.
		public static string current => ValueAt("ASPNETCORE_ENVIRONMENT");

		public static bool isDevelopment => current.EqualsIgnoreCaseDk(DEVELOPMENT);

		public static bool isStaging => current.EqualsIgnoreCaseDk(STAGING);

		public static bool isProduction => current.EqualsIgnoreCaseDk(PRODUCTION);

		private static string ValueAt(string key) {
			return Environment.GetEnvironmentVariable(key) ?? throw new NotSupportedException($"Invalid key: {key}");
		}
	}
}
