import React, { useState, useEffect } from 'react';
import SortableTable from './SortableTable';

function AllUploadsTable() {
    const [allUploads, setAllUploads] = useState([]);

    useEffect(() => {
        fetch('/api/v1/all')
            .then(response => response.json())
            .then(data => setAllUploads(data))
            .catch(error => console.error('Error fetching all uploads:', error));
    }, []);

    const parseTable = data => {
        return data.map(row => {
            const { uuid, fileSize, lastModified } = row;

            const uuid_href = `/upload/${uuid}`;
            const uuid_el = (<a href={uuid_href}>{uuid}</a>);

            return {
                uuid: uuid_el,
                fileSize,
                lastModified
            };
        });
    };

    return (
        <SortableTable data={parseTable(allUploads)} />
    );
}

export default AllUploadsTable;
