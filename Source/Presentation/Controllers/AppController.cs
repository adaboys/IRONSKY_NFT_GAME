using Microsoft.AspNetCore.Mvc;

namespace App {
	[ApiController, Route(RouteConst.api_prefix)]
	public class AppController : ControllerBase {
		private readonly AppService service;

		public AppController(AppService service) {
			this.service = service;
		}

		[HttpGet, Route(RouteConst.app_info)]
		public async Task<ActionResult<ApiResponse>> GetAppInfo(int os_id) {
			return await service.GetAppInfo(os_id);
		}
	}
}
