import NotFound from '@components/NotFound';

const NotFoundPage = () => {
    return <NotFound message={<>
        <p>
            We can’t seem to find a page you are looking for! {' '}
        </p>
        <p>
            You may have mistyped the address or the page may have moved. {" "}
        </p>
        <p>
            We’re sorry for the error and hope you’ll have a good day.
        </p>
    </>} />
}

export default NotFoundPage
