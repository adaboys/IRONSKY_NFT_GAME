using Microsoft.AspNetCore.Mvc;

/// This project is aimed to api, so we use `ControllerBase` since
/// it does not support View.

namespace App {
	public class BaseController : ControllerBase {
		/// Get current authenticated user's id.
		protected Guid? ClaimUserId() {
			try {
				var userId = this.User.FindFirst(AppConst.my_jwt_claim_user_id)?.Value ?? null;
				if (userId != null) {
					return new Guid(userId);
				}
			}
			catch (Exception) { }

			return null;
		}
	}
}
