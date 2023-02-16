// Current environment info.
class _Env {
	readonly version = process.env.VERSION!!;

	readonly debug = process.env.DEBUG === "true";

	readonly current = process.env.ENVIRONMENT;
	readonly isProduction = process.env.ENVIRONMENT === "production";
	readonly isStaging = process.env.ENVIRONMENT === "staging";
	readonly isDevelopment = process.env.ENVIRONMENT === "development";

	readonly ACCESS_KEY = process.env.ACCESS_KEY!!;
	readonly SECRET_KEY = process.env.SECRET_KEY!!;
	readonly REGION = process.env.REGION!!;
	readonly BUCKET_NAME = process.env.BUCKET_NAME!!;
}

/// Onetime initialization
export const Env = new _Env();
