using Tool.Compet.Core;

namespace App {
	public class AppService {
		private readonly AppDbContext dbContext;

		public AppService(AppDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public async Task<ApiResponse> GetAppInfo(int osId) {
			var app = dbContext.apps.Where(m => m.osId == osId).FirstOrDefault();
			if (app == null) {
				return new ApiNotFoundResponse();
			}

			return new AppResponse {
				data = new() {
					osId = app.osId,
					version = app.version,
				}
			};
		}
	}
}
