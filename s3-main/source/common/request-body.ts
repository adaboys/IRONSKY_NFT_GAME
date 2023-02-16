import { IsArray, IsBoolean, IsInt, IsNotEmpty, IsNotEmptyObject, Min, ValidateNested } from 'class-validator';

// Npm: https://www.npmjs.com/package/class-validator
// Src: https://github.com/typestack/class-validator#validation-decorators

export class CreateNewNftPolicyRequestBody {
	/**
	 * Number of elapsed second from the policy's creation time that causes the policy be invalid (cannot mint/burn via the policy).
	 * For infinity value, just pass -1.
	 */
	@IsInt()
	@Min(-1)
	invalid_after_seconds_elapsed: number;

	constructor(that: any) {
		this.invalid_after_seconds_elapsed = that.invalid_after_seconds_elapsed;
	}
}

export class MintAndBuyNftRequestBody {
	/**
	 * Normally it is project root user who can mint the NFT.
	 */
	@IsNotEmpty()
	seller_address: string;

	@IsNotEmpty()
	payer_address: string;

	@IsNotEmpty()
	receiver_address: string;

	/**
	 * Normally it is lovelace (ADA token).
	 * For another token, for eg,. ISKY, it should be token_id of ISKY coin at Cardano blockchain.
	 */
	@IsNotEmpty()
	pay_with_token: string;

	/**
	 * Amount of token that payer will pay to seller.
	 */
	@IsInt()
	@Min(0)
	price: number;

	/**
	 * If this is provided, then seller will pay the fee by subtract fee from the price.
	 * Otherwise, payer need pay the fee.
	 */
	@IsBoolean()
	discount_fee_from_price: boolean;

	@IsNotEmpty()
	nft_policy_id: string;

	@IsInt()
	@Min(0)
	invalid_after_seconds_elapsed: number;

	@IsInt()
	@Min(1)
	nft_quantity: number;

	@IsNotEmpty()
	nft_asset_name: string;

	@IsNotEmptyObject()
	nft_metadata: any;

	constructor(that: any) {
		this.seller_address = that.seller_address;
		this.payer_address = that.payer_address;
		this.receiver_address = that.receiver_address;
		this.pay_with_token = that.pay_with_token;
		this.price = that.price;
		this.discount_fee_from_price = that.discount_fee_from_price;
		this.nft_policy_id = that.nft_policy_id;
		this.invalid_after_seconds_elapsed = that.invalid_after_seconds_elapsed;
		this.nft_quantity = that.nft_quantity;
		this.nft_asset_name = that.nft_asset_name;
		this.nft_metadata = that.nft_metadata;
	}
}

export class CreateNewTokenPolicyRequestBody {
	/**
	 * Number of elapsed second from the policy's creation time that causes the policy be invalid (cannot mint/burn via the policy).
	 * For infinity value, just pass -1.
	 */
	@IsInt()
	@Min(-1)
	invalid_after_seconds_elapsed: number;

	constructor(that: any) {
		this.invalid_after_seconds_elapsed = that.invalid_after_seconds_elapsed;
	}
}

export class CreateTokenRequestBody {
	@IsNotEmpty()
	payer_address: string;

	@IsNotEmpty()
	receiver_address: string;

	@IsNotEmpty()
	token_policy_id: string;

	@IsNotEmpty()
	token_name: string;

	@IsInt()
	@Min(1)
	token_quantity: number;

	constructor(that: any) {
		this.payer_address = that.payer_address;
		this.receiver_address = that.receiver_address;
		this.token_policy_id = that.token_policy_id;
		this.token_name = that.token_name;
		this.token_quantity = that.token_quantity;
	}
}

export class MintTokenRequestBody {
	@IsNotEmpty()
	payer_address: string;

	@IsNotEmpty()
	receiver_address: string;

	@IsNotEmpty()
	token_id: string;

	@IsInt()
	@Min(1)
	token_quantity: number;

	constructor(that: any) {
		this.payer_address = that.payer_address;
		this.receiver_address = that.receiver_address;
		this.token_id = that.token_id;
		this.token_quantity = that.token_quantity;
	}
}

export class BurnTokenRequestBody {
	@IsNotEmpty()
	target_address: string;

	@IsNotEmpty()
	token_id: string;

	@IsInt()
	@Min(1)
	token_quantity: number;

	constructor(that: any) {
		this.target_address = that.target_address;
		this.token_id = that.token_id;
		this.token_quantity = that.token_quantity;
	}
}

export class AssetInfo {
	/**
	 * For eg,. lovelace, Token01kasldka.xalal
	 */
	@IsNotEmpty()
	asset_id: string;

	/**
	 * For eg,. 123000000 lovelace, 200000 Token01kasldka.xalal
	 */
	@IsInt()
	@Min(0)
	asset_quantity: number;

	constructor(that: any) {
		this.asset_id = that.asset_id;
		this.asset_quantity = that.asset_quantity;
	}
}

export class TxAssetsRequestBody {
	@IsNotEmpty()
	fee_payer_address: string;

	@IsArray()
	@ValidateNested()
	transactions: TransactionItem[];

	constructor(that: any) {
		this.fee_payer_address = that.fee_payer_address;

		this.transactions = [];
		for (const txItem of that.transactions) {
			this.transactions.push(new TransactionItem(txItem));
		}
	}
}

export class TransactionItem {
	@IsNotEmpty()
	sender_address: string;

	@IsNotEmpty()
	receiver_address: string;

	@IsArray()
	@ValidateNested()
	assets: AssetInfo[];

	constructor(that: any) {
		this.sender_address = that.sender_address;
		this.receiver_address = that.receiver_address;

		this.assets = [];
		for (const asset of that.assets) {
			this.assets.push(new AssetInfo(asset));
		}
	}
}

export class SendAssetsRequestBody {
	@IsNotEmpty()
	sender_address: string;

	@IsNotEmpty()
	receiver_address: string;

	@IsBoolean()
	sender_is_fee_payer: boolean;

	@IsArray()
	@ValidateNested()
	assets: AssetInfo[];

	constructor(that: any) {
		this.sender_address = that.sender_address;
		this.receiver_address = that.receiver_address;
		this.sender_is_fee_payer = that.sender_is_fee_payer;

		this.assets = [];
		for (const item of that.assets) {
			this.assets.push(new AssetInfo(item));
		}
	}
}

export class CreateWalletRequestBody {
	/**
	 * Normally it is email, or something unique that identifies over all clients.
	 */
	@IsNotEmpty()
	client_id: string;

	constructor(that: any) {
		this.client_id = that.client_id;
	}
}
