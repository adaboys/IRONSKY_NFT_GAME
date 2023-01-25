import { useRouter } from "next/router";

import Nfts from "@components/Marketplace/Nfts";
import Material from "@components/Marketplace/Material";
import Ticket from "@components/Marketplace/Ticket";
import NotFound from "@components/NotFound";

import { nftTypes } from "@constants";

const MarketplacePage = () => {
    const { query } = useRouter();
    switch (query?.slug) {
        case 'aircraft':
            return <Nfts nftType={nftTypes.AIRCRAFT} />
        case 'wingman':
            return <Nfts nftType={nftTypes.WINGMAN} />
        case 'commander':
            return <Nfts nftType={nftTypes.COMMANDER} />
        case 'material':
            return <Material />
        case 'ticket':
            return <Ticket />
        default:
            return <NotFound />
    }
}

export default MarketplacePage;