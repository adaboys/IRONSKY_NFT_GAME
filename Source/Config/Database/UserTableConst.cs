/// User constants which be shared between client and server.
/// It contains database constants at server side, and api constants for client side.

namespace App {
	public class UserTableConst {
		// For `gender`
		public const byte GENDER_MALE = 1;
		public const byte GENDER_FEMALE = 2;
		public const byte GENDER_OTHER = 3;

		// For `role`
		public const byte ROLE_USER = 10;
		public const byte ROLE_ADMIN = 90;
		public const byte ROLE_ROOT = 100;

		// For `status`
		public const byte STATUS_NORMAL = 0;
		public const byte STATUS_INVALID = 1;
	}
}
