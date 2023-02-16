const express = require('express');
const path = require('path');
const createError = require('http-errors');
const cookieParser = require('cookie-parser');
const logger = require('morgan');

import { Request, Response, NextFunction } from 'express';

// [Config]
// Use dotenv lib to handle with .env file.
import dotenv from 'dotenv';
dotenv.config();

// Import from here (after dotenv)
import { Env } from './common/env';
import { AppLogs } from './common/app-logs';

// [Import api routes]
import topRouter from './presentation/routes/top';
import S3Service from './business/s3-service';

const app = express();

// [Setup view engine]
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

// [Map api routes]
// For aboud source code structure, see below:
// https://sodocumentation.net/node-js/topic/10785/route-controller-service-structure-for-expressjs
app.use('/top', topRouter);

// Catch 404 and forward to error handler
app.use(function (req: Request, res: Response, next: NextFunction) {
	next(createError(404));
});

// Error handler
app.use(function (err: any, req: Request, res: Response, next: NextFunction) {
	AppLogs.Error(app, `${err.status || 500} - ${res.statusMessage} - ${err.message} - ${req.originalUrl} - ${req.method} - ${req.ip}`);

	res.json({
		status: 500,
		message: "Unhandled error"
	});

	// Below is default generated code by express.
	// Set locals, only providing error in development
	// res.locals.message = err.message;
	// res.locals.error = req.app.get('env') === 'development' ? err : {};
	// Render the error page
	// res.status(err.status || 500);
	// res.render('error');
});

module.exports = app;

// Perform upload (from local -> to s3)
const s3Service = new S3Service();
s3Service.UploadFolder("data/cm", "cm");
