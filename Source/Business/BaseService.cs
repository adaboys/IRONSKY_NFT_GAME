namespace App {
	public class BaseService {
		protected readonly AppDbContext dbContext;

		public BaseService(AppDbContext dbContext) {
			this.dbContext = dbContext;
		}

		protected async Task<UserModel?> FindUserById(Guid userId) {
			try {
				return this.dbContext.users.Where(u => u.id == userId).FirstOrDefault();
			}
			catch (Exception) {
				return null;
			}
		}
	}
}
