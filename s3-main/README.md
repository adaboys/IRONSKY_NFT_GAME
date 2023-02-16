# S3


## Quick start

- Build and Run server

	```bash
	# Goto server folder
	cd s3

	# Add env file
	cp .env.sample .env

	# [Optional] For development env, consider use `nodemon ./bin/www` instead of `node ./bin/www`
	nano package.json

	# Build
	# Note: when has any update, just run: npm update
	npm install

	# Start
	npm start
	```

## For developer

- Run test

	```bash
	# Ensure test file are created
	mkdir test && cd test
	nano test.ts
	npm run build

	# Run (or: npm test)
	mocha
	```
