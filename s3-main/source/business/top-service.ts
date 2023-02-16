import { ApiResponse, ApiSucceedResponse } from "../common/api-responses";

export default class TopService {
	async Upload(): Promise<ApiResponse> {
		return new ApiSucceedResponse();
	}
}
