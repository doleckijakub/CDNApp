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

    const testData = [
        { id: 1, name: 'John', age: 28, location: 'New York' },
        { id: 2, name: 'Jane', age: 22, location: 'London' },
        { id: 3, name: 'Doe', age: 34, location: 'Paris' },
    ];

    return (
        <SortableTable data={allUploads} />
        // <div>
        //     {/* <h2>All Uploads</h2>
        //     {allUploads.length === 0 ? (
        //         <p>No uploads yet...</p>
        //     ) : (
        //         <ul>
        //             {allUploads.map((uuid) => (
        //                 <li key={uuid}>
        //                     <a href={`/upload/${uuid}`}>{uuid}</a>
        //                 </li>
        //             ))}
        //         </ul>
        //     )} */}
        // </div>
    );
}

export default AllUploadsTable;
