using Microsoft.AspNetCore.Mvc;

namespace App {
	[ApiController, Route(RouteConst.api_prefix)]
	public class TopController : ControllerBase {
		// private readonly ILogger logger;

		// public TopController(ILogger logger) {
		// 	this.logger = logger;
		// }

		/// Note:
		/// - It does NOT work if we use empty route as `Route("")`
		/// - It does not work if we add prefix slash to route as `Route("/api/auth/login")`
		[HttpGet, Route("top")]
		public string Top() {
			return "Hellow, this is top page.";
		}
	}
}
