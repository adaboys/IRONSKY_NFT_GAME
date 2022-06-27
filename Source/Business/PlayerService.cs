namespace App {
	using Tool.Compet.Core;
	using Tool.Compet.Json;
	using Tool.Compet.Log;

	public class PlayerService : BaseService {
		private readonly IWebHostEnvironment env;

		public PlayerService(AppDbContext dbContext, IWebHostEnvironment env) : base(dbContext) {
			this.env = env;
		}

		public async Task<ApiResponse> VerifyPlayer(Guid userId, long playerId) {
			var player = dbContext.players.FirstOrDefault(m => m.id == playerId && m.userId == userId);
			if (player == null) {
				return new ApiBadRequestResponse("Player not found.");
			}
			return new ApiSuccessResponse();
		}

		public async Task<ApiResponse> RegisterPlayer(Guid userId, RegisterPlayerRequestBody request) {
			var user = await this.FindUserById(userId);
			if (user == null) {
				return new ApiUnauthorizedResponse("Invalid user");
			}

			var player = dbContext.players.FirstOrDefault(m => m.name == request.playerName);
			if (player != null) {
				return new ApiBadRequestResponse("Player name existed.");
			}

			player = new PlayerModel {
				userId = userId,
				name = request.playerName,
				level = PlayerTableConst.DEFAULT_LEVEL,
			};

			dbContext.players.Attach(player);
			await dbContext.SaveChangesAsync();

			return new RegisterPlayerResponse {
				data = new() {
					playerId = player.id,
					playerName = request.playerName
				}
			};
		}

		public async Task<ApiResponse> ChangePlayerName(Guid userId, long playerId, string newPlayerName) {
			var player = dbContext.players.Where(m => m.id == playerId && m.userId == userId).FirstOrDefault();
			if (player == null) {
				return new ApiNotFoundResponse("Not found player");
			}

			// Check newName is duplicated with other player's name
			var otherPlayer = dbContext.players
				.Where(m => m.name == newPlayerName && m.userId != userId)
				.FirstOrDefault();

			if (otherPlayer != null) {
				return new ApiBadRequestResponse("Name was existed");
			}

			// Update the player
			player.name = newPlayerName;
			// player.updatedAt = System.DateTime.Now;
			// dbContext.Entry(player).Property(model => model.name).IsModified = true;
			await dbContext.SaveChangesAsync();

			return new ApiSuccessResponse();
		}
	}
}
