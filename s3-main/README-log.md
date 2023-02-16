# Cardano node


## How this project was made

- Make `nodejs + express + typescript` project at local.\
Note: we install nodejs inside the compute engine to work with cardano-node to public apis.

	```bash
	# Create project with express generator
	# Ref: https://expressjs.com/en/starter/generator.html
	mkdir -p s3 && cd s3
	git init
	npx express-generator
	npm install

	# Setup typescript to work with [nodejs + express].
	# Note that: compiler will convert our ts code to js at development stage to build folder.
	# Ref: https://www.pullrequest.com/blog/intro-to-using-typescript-in-a-nodejs-express-project/
	npm install typescript ts-node @types/node @types/express --save-dev
	npx tsc --init

	# Install darkcompet modules
	npm install @darkcompet/js-core --save
	npm install @darkcompet/nodejs-core --save
	npm install @darkcompet/nodejs-s3 --save

	# Validator
	npm install class-validator --save
	# Logging
	npm install winston --save
	npm install winston-daily-rotate-file --save

	# To work with each env, we install dotenv
	# Ref: https://medium.com/the-node-js-collection/making-your-node-js-work-everywhere-with-environment-variables-2da8cdf6e786
	npm install dotenv --save

	# S3 sdk
	npm install multer aws-sdk body-parser --save

	# Json Web Token (for authentication)
	# npm install jsonwebtoken --save

	# Mocha for test
	# npm install mocha --save-dev
	# npm i @types/mocha --save-dev

	# [Optional] For quick development, we should enable auto-reload feature with nodemon
	- Run: npm install nodemon --save-dev
	- At `package.json`, change start script from `node ./bin/www` to `nodemon ./bin/www`

	# Start server, then browser to external_ip:8200
	npm start
	```


- Setup VM instance (use compute engine at gcloud)

	```bash
	# Just create new `compute engine` with below name.
	# Then start that instance.
	test-cardano-node-instance

	# Install gcloud lib at local.
	# Download gcloud for Apple M1.
	https://cloud.google.com/sdk/docs/install-sdk

	# Init and Link to our project at gcloud
	# And unzip the downloaded file to user home folder.
	~/google-cloud-sdk/install.sh
	~/google-cloud-sdk/bin/gcloud init

	# SSH to gcloud
	~/google-cloud-sdk/bin/gcloud compute ssh --zone "asia-southeast1-b" "test-cardano-node-instance"  --project "test-cardano-node-350809"

	# Setup firewall
	# To call api to the app (for eg,. nodejs app) which is running at compute engine,
	# we have to setup firewall to allow specific TCP port via `Setup firewall rules`.
	# For eg,. setup firewall for port 8200, or for all ports.
	```


- Setup cardano-node: `README-log-cardano-node.md`


- Setup nodejs server with nginx as proxy-reverse engine: `README-log-nodejs-server.md`



## Setup ssh for sites

- Setup ssh for marketplace site

	```bash
	# Ref: https://www.digitalocean.com/community/tutorials/how-to-secure-nginx-with-let-s-encrypt-on-ubuntu-22-04

	# Before start, ensure both of the following DNS records (at google domain site) set up for own server:
	- An A record with `ironsky-cardano-node.darkcompet.com` pointing to own server’s public IP address.
	- An A record with `www.ironsky-cardano-node.darkcompet.com` pointing to own server’s public IP address.


	# Install certbot
	# For uninstall: sudo apt remove certbot
	sudo snap install core
	sudo snap refresh core
	sudo snap install --classic certbot
	# [Optional] Above will auto link certbot command for us. But if not yet, just run:
	sudo ln -s /snap/bin/certbot /usr/bin/certbot
	# [Optional] For ubutn 20-, install with:
	sudo apt install certbot python3-certbot-nginx

	# Certbot needs to be able to find the correct server block in your Nginx configuration
	# for it to be able to automatically configure SSL. Specifically, it does this by looking for
	# a server_name directive that matches the domain you request a certificate for.
	sudo nano /etc/nginx/sites-available/ironsky-cardano-node.config
	# Make sure config files contain:
	# server_name ironsky-cardano-node.darkcompet.com www.ironsky-cardano-node.darkcompet.com;

	# Check grammar and Reload nginx
	sudo nginx -t
	# If failed, maybe we use long domain name??
	# just comment out and change 64 -> 128: server_names_hash_bucket_size 128;
	# at: /etc/nginx/nginx.conf

	# Reload nginx config
	sudo service nginx reload

	# Obtain ssh cert
	sudo certbot --nginx -d ironsky-cardano-node.darkcompet.com -d www.ironsky-cardano-node.darkcompet.com
	# Select 1 (reinstall existing cert)
	# Select 2 (redirect all requests from http to https).

	# Make cert is auto renewal by install crontab by default
	# By default, cert is only valid for 90 days.
	sudo systemctl status certbot.timer
	# [Optional] Renew all sites manually
	sudo certbot renew --dry-run
	```
