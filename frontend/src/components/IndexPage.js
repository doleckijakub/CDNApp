import CollapsibleSection from './CollapsibleSection';
import WebUploadForm from './WebUploadForm';
import PublicUrl from './PublicUrl';
import AllUploadsTable from './AllUploadsTable';

const IndexPage = () => {
  return (
    <div className="container mt-4">
      <header className="text-center mb-4">
        <h1 className="text-warning">CDN App</h1>
      </header>

      <main>
        <section id="help">
          <h2 className="text-warning border-bottom pb-2">About This App</h2>
          <p>
            Welcome to my CDN App. You can easily upload and share files with others. Here's a quick guide on how to use the app:
          </p>
          <CollapsibleSection title="Uploading files">
            <CollapsibleSection title="Using webupload">
              <WebUploadForm />
            </CollapsibleSection>
            <CollapsibleSection title="Linux / MacOS">
              <p>
                To upload a file called <code>file.txt</code> simply type the below command in your terminal
              </p>
              <code>
                curl <PublicUrl /> -T file.txt
              </code>
            </CollapsibleSection>
            <CollapsibleSection title="Windows">
              <p>
                To upload a file called <code>file.txt</code> simply type the below command in PowerShell
              </p>
              <code>
                Invoke-WebRequest -Uri "<PublicUrl />" -InFile "file.txt" -Method Put
              </code>
            </CollapsibleSection>
          </CollapsibleSection>
          <CollapsibleSection title="Downloading files">
            <p>
              To download a file you need its UUID and filename.
              To download a file with specific UUID and filename go to <a href="#"><PublicUrl />/upload/UUID</a> and click on the filename you want to download.
              You can also download it directly from <a href="#"><PublicUrl />/UUID/filename</a>.
              Additionally, feel free to browse the <a href="#recent-uploads">recent uploads</a> and find your upload there.
            </p>
          </CollapsibleSection>
          <p>If you encounter any issues or have any questions, please don't hesitate to <a href="https://github.com/doleckijakub">contact me</a>.</p>
        </section>
        <section id="recent-uploads">
          <h2 className="text-warning border-bottom pb-2">Recent Uploads</h2>
          <AllUploadsTable />
        </section>
      </main>

      <footer className="text-center mt-4 py-3">
        <p className="text-light"><a href="https://github.com/doleckijakub">doleckijakub</a>'s <a href="https://github.com/doleckijakub/CDNApp">CDN App</a>. <a href="https://github.com/doleckijakub/CDNApp/blob/master/LICENSE">MIT Licence</a>.</p>
      </footer>
    </div>
  );
};

export default IndexPage;