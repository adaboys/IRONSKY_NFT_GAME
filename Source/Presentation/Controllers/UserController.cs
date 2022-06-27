using Microsoft.AspNetCore.Mvc;

namespace App {
	[ApiController, Route(RouteConst.api_prefix)]
	public class UserController : BaseController {
		private readonly UserService service;

		public UserController(UserService service) {
			this.service = service;
		}

		[HttpPost, Route(RouteConst.user_createWithExternalWallet)]
		public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] CreateUserRequestBody requestBody) {
			return await this.service.RegisterUserWithExternalWallet(requestBody);
		}
	}
}
