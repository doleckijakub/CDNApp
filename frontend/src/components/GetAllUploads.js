import React, { useState, useEffect } from 'react';

function GetAllUploads() {
    const [allUploads, setAllUploads] = useState([]);

    useEffect(() => {
        fetch('/api/v1/all')
            .then(response => response.json())
            .then(data => setAllUploads(data))
            .catch(error => console.error('Error fetching all uploads:', error));
    }, []);

    return (
        <div>
            <h2>All Uploads</h2>
            {allUploads.length === 0 ? (
                <p>No uploads yet...</p>
            ) : (
                <ul>
                    {allUploads.map((uuid) => (
                        <li key={uuid}>
                            <a href={`/upload/${uuid}`}>{uuid}</a>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default GetAllUploads;
