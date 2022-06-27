namespace App {
	using System.IdentityModel.Tokens.Jwt;
	using System.Security.Claims;
	using System.Text;
	using Microsoft.IdentityModel.Tokens;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Options;
	using Tool.Compet.Core;
	using Tool.Compet.Log;

	public class AuthService {
		private readonly AppSetting appSetting;
		private readonly AppDbContext dbContext;

		public AuthService(IOptions<AppSetting> appSettingOpt, AppDbContext dbContext) {
			this.appSetting = appSettingOpt.Value;
			this.dbContext = dbContext;
		}

		public async Task<ApiResponse> VerifyAuth(string accessToken) {
			var userId = this.ValidateAccessToken(accessToken);
			if (userId == null) {
				return new ApiUnauthorizedResponse("Invalid token");
			}

			var user = dbContext.users.FirstOrDefault(m => m.id == userId && m.status == UserTableConst.STATUS_NORMAL);
			if (user == null) {
				return new ApiUnauthorizedResponse("User not found");
			}

			return new VerifyAuthResponse {
				data = new() {
					userId = ((Guid)userId).ToStringDk()
				}
			};
		}

		public async Task<ApiResponse> LoginWithIdAndPassword(LoginRequestBody request) {
			// Since user can login with email/password or code/password,
			// we check them with request's id.
			var user = dbContext.users
				// Note that: below LinQ expression is not executed, it is just statement
				// which will be translated to real query.
				// So lambda expression should not return value here.
				.Where(u => u.email == request.id || u.code == request.id)
				.FirstOrDefault() // Do NOT use First() since exception was thrown if not found.
			;

			if (user == null) {
				return new ApiBadRequestResponse("Not found user");
			}

			// Check password match or not
			var passwordHasher = new PasswordHasher<UserModel>();
			if (passwordHasher.VerifyHashedPassword(user, user.password, request.password) != PasswordVerificationResult.Success) {
				return new ApiUnauthorizedResponse();
			}

			var accessToken = this.GenerateAccessToken(user.id);

			return new LoginResponse {
				data = new() {
					accessToken = accessToken
				}
			};
		}

		public async Task<ApiResponse> LogInWithProvider(ProviderLoginRequestBody request) {
			var providerUser = request.provider switch {
				"google" => await this.GetUserFromGoogle(request.accessToken),
				"facebook" => await this.GetUserFromFacebook(request.accessToken),
				_ => throw new Exception("Invalid provider")
			};

			var user = new UserModel {
				email = providerUser.email,
			};

			await dbContext.users.AddAsync(user);

			var accessToken = this.GenerateAccessToken(user.id);

			return new LoginResponse {
				data = new() {
					accessToken = accessToken
				}
			};
		}

		public async Task<ApiResponse> LogOut(Guid userId, LogoutRequestBody logoutRequest) {
			return new ApiSuccessResponse();
		}

		/// @param `userClaimValue`: For more security, pass it as user's unique info as user_id.
		/// @param `aliveSeconds`: By default, access token will valid for 1 year.
		private string GenerateAccessToken(Guid userId, long aliveSeconds = 365 * 24 * 3600) {
			if (DkBuildConfig.DEBUG) { Tool.Compet.Log.DkLogs.Debug(this, $"Gen token -> userId: {userId.ToStringDk()}"); }

			// We use `N` when convert Guid to 32-char string without hyphen.
			// See: https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=net-6.0
			var claims = new[] {
				new Claim(JwtRegisteredClaimNames.Sub, appSetting.jwt.subject),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
				new Claim(AppConst.my_jwt_claim_user_id, userId.ToStringDk()),
			};

			var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSetting.jwt.key));
			var securityToken = new JwtSecurityToken(
				issuer: appSetting.jwt.issuer,
				audience: appSetting.jwt.audience,
				claims: claims,
				expires: DateTime.Now.AddSeconds(aliveSeconds),
				signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
			);

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}

		/// @return Nullable userId in Guid instance.
		private Guid? ValidateAccessToken(string accessToken) {
			try {
				new JwtSecurityTokenHandler().ValidateToken(accessToken, new TokenValidationParameters {
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSetting.jwt.key)),

					ValidateIssuer = true,
					ValidIssuer = appSetting.jwt.issuer,

					ValidateAudience = true,
					ValidAudience = appSetting.jwt.audience,

					// Set clockskew to zero so tokens expire exactly at
					// token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out var validatedToken);

				// [
				// 	{
				// 		"Issuer": "server",
				// 		"OriginalIssuer": "server",
				// 		"Properties": {},
				// 		"Subject": null,
				// 		"Type": "sub",
				// 		"Value": "token",
				// 		"ValueType": "http://www.w3.org/2001/XMLSchema#string"
				// 	},
				// 	{
				// 		"Issuer": "server",
				// 		"OriginalIssuer": "server",
				// 		"Properties": {},
				// 		"Subject": null,
				// 		"Type": "jti",
				// 		"Value": "99a63153-cf32-40e7-b7a8-ccc55104c494",
				// 		"ValueType": "http://www.w3.org/2001/XMLSchema#string"
				// 	},
				// 	{
				// 		"Issuer": "server",
				// 		"OriginalIssuer": "server",
				// 		"Properties": {},
				// 		"Subject": null,
				// 		"Type": "iat",
				// 		"Value": "01/09/2022 09:16:46",
				// 		"ValueType": "http://www.w3.org/2001/XMLSchema#string"
				// 	},
				// 	"Issuer": "server",
				// 	{
				// 		"OriginalIssuer": "server",
				// 		"Properties": {},
				// 		"Subject": null,
				// 		"Type": "my_jwt_claim_key",
				// 		"Value": "4e4f2530-f203-4e38-ac82-e55d5767ad12",
				// 		"ValueType": "http://www.w3.org/2001/XMLSchema#string"
				// 	},
				// 	...
				// ]
				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = jwtToken.Claims.First(claim => claim.Type == AppConst.my_jwt_claim_user_id).Value;
				if (userId == null) {
					return null;
				}

				if (DkBuildConfig.DEBUG) { Tool.Compet.Log.DkLogs.Debug(this, $"Validate token -> userId: {userId}"); }

				return new Guid(userId);
			}
			catch (Exception e) {
				if (BuildConfig.DEBUG) {
					DkLogs.Warning(this, $"ValidateAccessToken error: {e.Message}");
				}
			}

			return null;
		}

		private async Task<ProviderUser> GetUserFromGoogle(string token) {
			var result = new ProviderUser();
			//dktodo ask google
			return result;
		}

		private async Task<ProviderUser> GetUserFromFacebook(string token) {
			var result = new ProviderUser();
			//dktodo ask facebook
			return result;
		}

		private class ProviderUser {
			public string email;
		}
	}
}
