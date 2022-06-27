namespace App {
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[ApiController, Route(RouteConst.api_prefix)]
	public class AuthController : BaseController {
		private readonly AuthService service;

		public AuthController(AuthService service) {
			this.service = service;
		}

		[HttpPost, Route(RouteConst.auth_verify)]
		public async Task<ActionResult<ApiResponse>> VerifyAuth([FromBody] VerifyAuthRequestBody request) {
			return await service.VerifyAuth(request.accessToken);
		}

		[HttpPost, Route(RouteConst.auth_login)]
		public async Task<ActionResult<ApiResponse>> LoginWithIdAndPassword([FromBody] LoginRequestBody request) {
			return await service.LoginWithIdAndPassword(request);
		}

		[HttpPost, Route(RouteConst.auth_provider_login)]
		public async Task<ActionResult<ApiResponse>> LogInWithProvider([FromBody] ProviderLoginRequestBody loginRequest) {
			// Thanks to ASP.NET CORE for auto convert our result type to client's request Accept-Content type.
			// By default, it implicit convert result to ActionResult<>
			// Note that, `await` will implicit convert to Task<>
			// See https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0
			return await service.LogInWithProvider(loginRequest);
		}

		[Authorize]
		[HttpPost, Route(RouteConst.auth_logout)]
		public async Task<ActionResult<ApiResponse>> LogOut([FromBody] LogoutRequestBody request) {
			var userId = this.ClaimUserId();
			if (userId == null) {
				return new ApiUnauthorizedResponse();
			}
			// Implicit convert to Task<ActionResult<>>
			return await service.LogOut((Guid)userId, request);
		}
	}
}
