using Microsoft.Extensions.Options;
using Tool.Compet.Http;

namespace App {
	public class BaseRepo {
		private readonly AppSetting appSetting;

		public BaseRepo(IOptions<AppSetting> appSettingOpt) {
			this.appSetting = appSettingOpt.Value;
		}

		protected DkHttpClient httpClient {
			get {
				// Don't make it static since each task holds different request setting.
				var httpClient = new DkHttpClient();

				// We always attach bearer token to header for all apis even though some api does not need.
				httpClient.SetRequestHeaderEntry("Authorization", $"Bearer test");

				return httpClient;
			}
		}

		/// Calculate url for api.
		protected string CardanoNodeApiUrl(string relativePath) {
			if (relativePath.StartsWith('/')) {
				relativePath = relativePath.TrimStart('/');
			}

			return $"{this.appSetting.cardanoNode.apiBaseUrl}/{relativePath}";
		}
	}
}
