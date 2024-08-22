import React from 'react';

const WebUploadForm = () => {
    const handleSubmit = async (event) => {
        event.preventDefault();

        const fileInput = event.target.elements.fileUpload;
        const file = fileInput.files[0];

        if (!file) {
            alert('Please select a file to upload.');
            return;
        }

        try {
            const response = await fetch(`/${encodeURIComponent(file.name)}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': file.type || 'application/octet-stream',
                },
                body: file,
            });

            if (response.ok) {
                const result = await response.json();

                const uuid = `${result.uuid}`;
                const filename = `${result.filename}`;

                const directUrl =  `${document.location.origin}/${uuid}/${filename}`;
                const websiteUrl = `${document.location.origin}/upload/${uuid}`;
                
                alert(`File uploaded successfully!\n\nDirect URL: ${directUrl}\nWebsite URL: ${websiteUrl}`);
            } else {
                alert('File upload failed.');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('An error occurred during the file upload.');
        }
    };

    return (
        <form id="webupload" onSubmit={handleSubmit}>
            <div className="mb-3">
                <label htmlFor="fileUpload" className="form-label">Select file to upload:</label>
                <input type="file" className="form-control" id="fileUpload" name="fileUpload" required />
            </div>
            <button type="submit" className="btn btn-warning">Upload</button>
        </form>
    );
};

export default WebUploadForm;
