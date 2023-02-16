import *  as  winston from 'winston';
import 'winston-daily-rotate-file';

import { Env } from './env';

const level_default = "default";

const type_debug = "DEBUG";
const type_info = "INFO";
const type_notice = "NOTICE";
const type_warning = "WARNING";
const type_error = "ERROR";
const type_critical = "CRITICAL";

/// Logging: https://www.section.io/engineering-education/logging-with-winston/
/// Daily Rotate: https://github.com/winstonjs/winston-daily-rotate-file
class _Logger {
	_logger: winston.Logger;

	constructor() {
		this._logger = winston.createLogger({
			// We use one level since we don't want to log more other levels.
			// Note: when log a level (for eg,. 5), winston will also log lower level (<= 5).
			level: level_default,
			// Customize log levels (we use only default level)
			levels: { [level_default]: 0 },

			// Log format: 2022-07-19 20:30:59: TopController~ Hello, metadata: {"x": 1, "y": "something"}
			format: this._ConfigLogFormat(),

			// Send log to target place (we use file to store log)
			transports: [
				// Rotate file per day.
				// Just change datePattern to separate per hour, minute, second,...
				new winston.transports.DailyRotateFile({
					level: level_default,
					filename: 'logs/default/log-%DATE%.txt',
					datePattern: 'YYYY-MM-DD',
					zippedArchive: true,
					maxSize: '100m',
					maxFiles: '100d'
				}),
			],

			// Use winston daily-rotate-file to log unhandled exceptions.
			exceptionHandlers: [
				new winston.transports.DailyRotateFile({
					filename: 'logs/unhandled/exception-%DATE%.log',
					datePattern: 'YYYY-MM-DD',
					zippedArchive: true,
					maxSize: '100m',
					maxFiles: '100d'
				}),
			],
		});

		// For non-production env, also log to the `console` with simple format:
		// `${info.level}: ${info.message} JSON.stringify({ ...rest }) `
		if (!Env.isProduction) {
			this._logger.add(new winston.transports.Console({
				// format: winston.format.simple(),
				format: this._ConfigLogFormat(),
			}));
		}
	}

	_ConfigLogFormat(): winston.Logform.Format {
		return winston.format.combine(
			winston.format.timestamp({
				format: "YYYY-MM-DD HH:mm:ss"
			}),
			// Note that, when logging, we have associated `dump_obj` key to the metadata object.
			// So we can get that dump_obj in json, and display here.
			winston.format.printf(item => `${[item.timestamp]}: ${item.message}. Dump obj: ${JSON.stringify(item.dump_obj)}`),
		)
	}

	Debug(where: object, message: string, ...metadata: any[]) {
		this._logger.log(level_default, this._MakeLogMessage(type_debug, where, message), { dump_obj: metadata });
	}

	Info(where: object, message: string, ...metadata: any[]) {
		this._logger.log(level_default, this._MakeLogMessage(type_info, where, message), { dump_obj: metadata });
	}

	Notice(where: object, message: string, ...metadata: any[]) {
		this._logger.log(level_default, this._MakeLogMessage(type_notice, where, message), { dump_obj: metadata });
	}

	Warning(where: object, message: string, ...metadata: any[]) {
		this._logger.log(level_default, this._MakeLogMessage(type_warning, where, message), { dump_obj: metadata });
	}

	Error(where: object, message: string, ...metadata: any[]) {
		this._logger.log(level_default, this._MakeLogMessage(type_error, where, message), { dump_obj: metadata });
	}

	Critical(where: object, message: string, ...metadata: any[]) {
		this._logger.log(level_default, this._MakeLogMessage(type_critical, where, message), { dump_obj: metadata });
	}

	private _MakeLogMessage(logType: string, where: object, message: string): string {
		return `[${logType}] ${where.constructor.name}~ ${message}`;
	}
}

export const AppLogs = new _Logger();
