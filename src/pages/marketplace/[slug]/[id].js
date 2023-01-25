import NftDetail from "@components/Marketplace/NftDetail";
import { nftActions } from "@redux/actions";
import { wrapper } from '@redux/store';
import { nftTypes } from "@constants";

const NftDetailPage = () => {
    return (
        <NftDetail/>
    )
}

NftDetailPage.getInitialProps = wrapper.getInitialPageProps(store => ({ query }) => {
    const getNftDetail = {
        [nftTypes.AIRCRAFT]: nftActions.getAircraftDetail,
        [nftTypes.WINGMAN]: nftActions.getWingmanDetail,
        [nftTypes.COMMANDER]: nftActions.getCommanderDetail,
    }[query.slug];

    store.dispatch(getNftDetail({ id: query.id }));
});

export default NftDetailPage;