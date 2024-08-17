import React, { useState, useEffect } from 'react';
import SortableTable from './SortableTable';

function UploadFileList({ uuid }) {
    const [uploads, setUploads] = useState([]);

    useEffect(() => {
        fetch(`/api/v1/filesof/${uuid}`)
            .then(response => response.json())
            .then(data => setUploads(data))
            .catch(error => console.error('Error fetching files of upload:', error));
    }, [uuid]);

    const parseTable = data => {
        return data.map(row => {
            const { filename, fileSize } = row;

            const file_download_url = `/${uuid}/${filename}`;
            const filename_el = (<a href={file_download_url}>{filename}</a>);

            return {
                filename: filename_el,
                fileSize
            };
        });
    };

    return (
        // <div>
        //     <span>Files of this upload: {uuid}</span>
        //     {uploads.length === 0 ? (
        //         <p>No files found for this upload.</p>
        //     ) : (
        //         <ul>
        //             {uploads.map((filename) => (
        //                 <li key={filename}>
        //                     <a href={`/${uuid}/${filename}`} download>{filename}</a>
        //                 </li>
        //             ))}
        //         </ul>
        //     )}
        // </div>
        <SortableTable data={parseTable(uploads)} />
    );
}

export default UploadFileList;
