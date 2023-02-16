/// Api status code list: https://restfulapi.net/http-status-codes/

export class ApiResponse {
	status: number;
	message: string|null;

	// Only response when they are assigned.
	private code?: string | undefined;
	private data?: any | undefined;

	constructor(status: number, message: string|null) {
		this.status = status;
		this.message = message;
	}

	Code(code?: string): this {
		this.code = code;
		return this;
	}

	Data(data?: any): ApiResponse {
		this.data = data;
		return this;
	}
}

export class ApiSucceedResponse extends ApiResponse {
	constructor(message = "Succeed") {
		super(200, message);
	}
}

export class ApiBadRequestResponse extends ApiResponse {
	constructor(message = "Bad request") {
		super(400, message);
	}
}

export class ApiUnauthorizedResponse extends ApiResponse {
	constructor(message = "Unauthorized") {
		super(401, message);
	}
}

export class ApiForbiddenResponse extends ApiResponse {
	constructor(message = "Forbidden") {
		super(403, message);
	}
}

export class ApiNotFoundResponse extends ApiResponse {
	constructor(message = "Not found") {
		super(404, message);
	}
}

export class ApiInternalServerErrorResponse extends ApiResponse {
	constructor(message = "Internal server error") {
		super(500, message);
	}
}

export class ApiServiceUnavailableResponse extends ApiResponse {
	constructor(message = "Service unavailable") {
		super(503, message);
	}
}
