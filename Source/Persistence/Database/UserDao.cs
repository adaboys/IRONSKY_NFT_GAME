namespace App {
	public class UserDao {
		private readonly AppDbContext dbContext;

		public UserDao(AppDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public async Task<UserModel?> FindById(Guid userId, byte? status = UserTableConst.STATUS_NORMAL) {
			var query = this.dbContext.users.Where(model => model.id == userId);
			if (status != null) {
				query.Where(model => model.status == status);
			}
			return query.FirstOrDefault();
		}
	}
}
