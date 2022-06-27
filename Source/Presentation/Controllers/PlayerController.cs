using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App {
	[ApiController, Route(RouteConst.api_prefix)]
	public class PlayerController : BaseController {
		private readonly PlayerService service;

		public PlayerController(PlayerService service) {
			this.service = service;
		}

		[Authorize]
		[HttpGet, Route(RouteConst.player_verify)]
		public async Task<ActionResult<ApiResponse>> VerifyPlayer(long player_id) {
			var userId = this.ClaimUserId();
			if (userId == null) {
				return new ApiUnauthorizedResponse();
			}
			return await service.VerifyPlayer((Guid)userId, player_id);
		}

		[Authorize]
		[HttpPost, Route(RouteConst.player_register)]
		public async Task<ActionResult<ApiResponse>> RegisterPlayer([FromBody] RegisterPlayerRequestBody request) {
			var userId = this.ClaimUserId();
			if (userId == null) {
				return new ApiUnauthorizedResponse();
			}
			return await service.RegisterPlayer((Guid)userId, request);
		}

		[Authorize]
		[HttpPost, Route(RouteConst.player_name_change)]
		public async Task<ActionResult<ApiResponse>> SavePlayerName([FromBody] ChangePlayerNameRequestBody request) {
			var userId = this.ClaimUserId();
			if (userId == null) {
				return new ApiUnauthorizedResponse();
			}
			return await service.ChangePlayerName((Guid)userId, request.playerId, request.newPlayerName);
		}
	}
}
