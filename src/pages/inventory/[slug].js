import General from "@components/Inventory/General";
import NotFound from "@components/NotFound";
import { useRouter } from "next/router";

const InventorySubPage = () => {
    const { query } = useRouter();
    switch(query?.slug) {
        case 'general':
            return <General/>
        default:
            return <NotFound/>
    }
}

export default InventorySubPage;