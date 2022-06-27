using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace App {
	public static class JwtAuthenticationConfig {
		public static void ConfigureJwtAuthenticationDk(this IServiceCollection me, AppSetting appSetting) {
			me.AddAuthentication(options => {
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options => {
					options.RequireHttpsMetadata = true;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters {
						ValidateIssuer = true,
						ValidIssuer = appSetting.jwt.issuer,

						ValidateAudience = true,
						ValidAudience = appSetting.jwt.audience,

						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSetting.jwt.key))
					};
				});
		}
	}
}
