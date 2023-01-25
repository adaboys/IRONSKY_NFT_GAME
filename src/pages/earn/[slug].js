import { useRouter } from 'next/router';

import Staking from '@components/Earn/Staking';
import Farming from '@components/Earn/Farming';
import Event from '@components/Earn/Event';
import NotFound from '@components/NotFound';

const ToEarnPage = () => {
    const { query } = useRouter();
    switch (query?.slug) {
        case 'staking':
            return <Staking />;
        case 'farming':
            return <Farming />;
        case 'event':
            return <Event />;
        default:
            return <NotFound />
    }
}

export default ToEarnPage;