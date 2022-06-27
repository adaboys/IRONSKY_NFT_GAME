namespace App {
	public class UserService {
		private readonly AppDbContext dbContext;
		private readonly CardanoNodeRepo cardanoNodeRepo;

		public UserService(AppDbContext dbContext, CardanoNodeRepo cardanoNodeRepo) {
			this.dbContext = dbContext;
			this.cardanoNodeRepo = cardanoNodeRepo;
		}

		public async Task<ApiResponse> RegisterUserWithExternalWallet(CreateUserRequestBody requestBody) {
			var internalWalletRes = await this.cardanoNodeRepo.CreateInternalWallet(requestBody);

			if (internalWalletRes.succeed) {
				// Add new user
				var newUser = new UserModel();

				this.dbContext.users.Attach(newUser);
				await this.dbContext.SaveChangesAsync();

				// Add new user's wallet
				var newUserWallet = new UserWalletModel {
					userId = newUser.id,
					internalWalletAddress = internalWalletRes.data.internalWalletAddress,
					externalWalletAddress = requestBody.externalWalletAddress
				};

				this.dbContext.userWallets.Attach(newUserWallet);
				await this.dbContext.SaveChangesAsync();

				return new CreateUserResponse {
					data = new() {
						internalWalletAddress = newUserWallet.internalWalletAddress
					}
				};
			}

			return new ApiInternalServerErrorResponse();
		}
	}
}
