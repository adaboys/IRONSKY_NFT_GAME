import { Request, Response, NextFunction } from 'express';
import TopService from '../../business/top-service';
import { ApiResponse, ApiSucceedResponse } from '../../common/api-responses';
import { Env } from '../../common/env';

const topService = new TopService();

export default class TopController {
	async Get(req: Request, res: Response, next: NextFunction) {
		try {
			// Turn on before release
			// return res.status(404).send();

			// Turn off before release
			return res.json(new ApiSucceedResponse(`${Env.current}-${Env.version}-${Env.debug}`));
		}
		catch (e: any) {
			return res.json(new ApiResponse(e.status ?? 500, Env.debug ? e.message : "Failed"));
		}
	}

	async Post(req: Request, res: Response, next: NextFunction) {
		try {
			// Turn on before release
			// return res.status(404).send();

			// Turn off before release
			return res.json({
				status: 200,
				message: "OK",
				env: Env.current,
				version: Env.version,
				debug: Env.debug,
			});
		}
		catch (e: any) {
			return res.json(new ApiResponse(e.status ?? 500, Env.debug ? e.message : "Failed"));
		}
	}
}
