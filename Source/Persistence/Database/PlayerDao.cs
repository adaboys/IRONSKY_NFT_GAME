namespace App {
	public class PlayerDao {
		private readonly AppDbContext dbContext;

		public PlayerDao(AppDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public async Task<PlayerModel?> FindById(Guid userId, long playerId) {
			var query = this.dbContext.players.Where(p => p.id == playerId && p.userId == userId);
			return query.FirstOrDefault();
		}
	}
}
