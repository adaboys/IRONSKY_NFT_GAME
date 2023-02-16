import { DkFiles, DkUnixShell } from "@darkcompet/nodejs-core";
import AWS from "aws-sdk";
import { DeleteObjectsRequest, GetObjectRequest, ListObjectsV2Request } from "aws-sdk/clients/s3";
import fs from "fs";
import fs_promise from "fs/promises";
import path, { dirname } from "path";

import { ApiBadRequestResponse, ApiResponse, ApiSucceedResponse } from "../common/api-responses";
import { Env } from "../common/env";

// S3 config
AWS.config.update({
	accessKeyId: Env.ACCESS_KEY,
	secretAccessKey: Env.SECRET_KEY,
	region: Env.REGION
});

// Initialize S3 client
const s3 = new AWS.S3({ signatureVersion: 'v4' });

export default class S3Service {
	constructor() {
	}

	async ListFile(folderRelativePath: string = "wm") {
		const params: ListObjectsV2Request = {
			Bucket: Env.BUCKET_NAME,
			Prefix: folderRelativePath,
		};

		const result = await s3.listObjectsV2(params).promise();

		result.Contents?.forEach((content) => {
			console.log("File name: " + content.Key);
		});
	}

	// Upload local (src) -> S3 (dst)
	async UploadFolder(
		localSrcFolder_Rpath = "data/assets/image/bill",
		s3DstFolder_Rpath = "assets/image/bill"
	): Promise<ApiResponse> {
		// Note: __dirname = process.cwd() + "/dist"
		const folderPath = path.join(__dirname, "..", localSrcFolder_Rpath);
		console.log("folderPath: " + folderPath);
		console.log("BUCKET_NAME: " + Env.BUCKET_NAME);

		// Get of list of files from 'dist' directory
		const fileNames = await fs_promise.readdir(folderPath);
		if (!fileNames || fileNames.length === 0) {
			console.log(`Aborted. Folder '${folderPath}' is empty or does not exist.`);
			return new ApiBadRequestResponse();
		}
		if (fileNames.length > 10000) {
			console.error(`Aborted. It is recommended to upload under 10k files since S3 time-diff is only 15 minutes`);
			return new ApiBadRequestResponse();
		}

		// Upload each file
		let progress = 0;
		let totalFileCount = fileNames.length;
		for (const fileName of fileNames) {
			const filePath = path.join(folderPath, fileName);

			// Ignore folder, non-png
			if (await DkUnixShell.DirectoryExisted(filePath)) {
				console.log("Ignore upload sub-folder: " + filePath);
				--totalFileCount;
				continue;
			}
			if (!fileName.endsWith(".png")) {
				console.log("Ignore upload non-png file: " + filePath);
				--totalFileCount;
				continue;
			}

			// Async put to s3
			const failedFilePaths: any[] = [];
			const fileBuffer = await fs_promise.readFile(filePath);

			const uploadParams = {
				Bucket: Env.BUCKET_NAME,
				Key: `${s3DstFolder_Rpath}/${fileName}`,
				Body: fileBuffer
			};

			// Start upload async
			s3.putObject(uploadParams, (err, data) => {
				++progress;

				if (err) {
					failedFilePaths.push(filePath);
					console.error(`[${progress}/${totalFileCount}] Could NOT upload ${fileName}, error: ${JSON.stringify(err)}`);
				}
				else {
					console.log(`[${progress}/${totalFileCount}] Uploaded ${fileName}`);
				}

				if (progress == totalFileCount) {
					if (failedFilePaths.length > 0) {
						console.error("Failed file paths: " + failedFilePaths.join(", "));
					}
					else {
						console.log(`Uploaded ${totalFileCount} files successfully !`);
					}
				}
			});

			// For continuous upload:
			// const uploadResult = await s3.putObject(uploadParams).promise();
			// console.log("uploadResult: " + JSON.stringify(uploadResult));
		}

		return new ApiSucceedResponse();
	}

	async DownloadFolder(folderRelativePath: string = "ac-part") {
		const params: ListObjectsV2Request = {
			Bucket: Env.BUCKET_NAME,
			Prefix: folderRelativePath,
		};

		const listResult = await s3.listObjectsV2(params).promise();
		listResult.Contents?.forEach(async (content) => {
			console.log("Going to download file: " + content.Key);

			// Download
			const downloadParams: GetObjectRequest = {
				Bucket: Env.BUCKET_NAME,
				Key: content.Key!
			};

			const rootPath = process.cwd(); // __dirname
			const out_filePath = path.join(rootPath, "data", content.Key!);
			const out_dirPath = dirname(out_filePath);
			await DkFiles.MkDirsOrThrowAsync(out_dirPath);

			const readStream = s3.getObject(downloadParams).createReadStream();
			const writeStream = fs.createWriteStream(out_filePath);
			readStream.pipe(writeStream);
		});
	}

	async DeleteFolder(folderRelativePath: string = "wm") {
		const params: ListObjectsV2Request = {
			Bucket: Env.BUCKET_NAME,
			Prefix: folderRelativePath,
		};

		// Obtain file list for deletion
		const listResult = await s3.listObjectsV2(params).promise();
		listResult.Contents?.forEach((content) => {
			console.log("Going to delete file: " + content.Key);
		});

		// Delete files
		const deleteParams: DeleteObjectsRequest = {
			Bucket: Env.BUCKET_NAME,
			Delete: {
				Objects: []
			}
		};
		listResult.Contents?.forEach((content) => {
			deleteParams.Delete.Objects.push({
				Key: content.Key!
			});
		});

		const deleteResult = await s3.deleteObjects(deleteParams).promise();
		deleteResult.Deleted?.forEach((deletedObj) => {
			console.log("Deleted file: " + deletedObj.Key);
		});

		// If has more files, continue to delete
		if (listResult.IsTruncated) {
			console.log("Recursively delete remaining files...");
			await this.DeleteFolder(folderRelativePath);
		}
	}
}
