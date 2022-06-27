
namespace App {
	using Microsoft.Extensions.Options;

	public class CardanoNodeRepo : BaseRepo {
		public CardanoNodeRepo(IOptions<AppSetting> appSettingOpt) : base(appSettingOpt) {
		}

		public async Task<CreateInternalWalletResponse> CreateInternalWallet(CreateUserRequestBody body) {
			if (BuildConfig.DEBUG) {
				return new CreateInternalWalletResponse {
					data = new() {
						internalWalletAddress = "test_internal_wallet_addr"
					}
				};
			}
			return await this.httpClient.Post<CreateInternalWalletResponse>(CardanoNodeApiUrl(RouteConst.CardanoNode.wallet_create), body);
		}
	}
}
