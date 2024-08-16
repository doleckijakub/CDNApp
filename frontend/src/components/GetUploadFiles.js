import React, { useState, useEffect } from 'react';

function GetUploadFiles({ uuid }) {
    const [uploads, setUploads] = useState([]);

    useEffect(() => {
        fetch(`/api/v1/filesof/${uuid}`)
            .then(response => response.json())
            .then(data => setUploads(data))
            .catch(error => console.error('Error fetching files of upload:', error));
    }, [uuid]);

    return (
        <div>
            <h2>Files of this upload: {uuid}</h2>
            {uploads.length === 0 ? (
                <p>No files found for this upload.</p>
            ) : (
                <ul>
                    {uploads.map((filename) => (
                        <li key={filename}>
                            <a href={`/${uuid}/${filename}`} download>{filename}</a>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default GetUploadFiles;
