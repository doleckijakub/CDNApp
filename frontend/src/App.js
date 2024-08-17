import React, { useState, useEffect } from 'react';
import AllUploadsTable from './components/AllUploadsTable';
import UploadFileList from './components/UploadFileList';
import './App.css';

/*
 * It seems like I had to write a "Router" myself, because for some reason, 
 * combining a React frontend and a .NET backend is not easy. And either way
 * I like to write stuff by myself.
 * 
 * Perhaps in the future I will try to make it "As it should be", but it isn't
 * a big issue for me right now.
 */ 

const App = () => {
  const [component, setComponent] = useState(null);

  useEffect(() => {
    const renderComponentBasedOnPath = () => {
      const path = window.location.pathname;
      
      if (path === '/') {
        setComponent(<AllUploadsTable />);
      } else if (path.startsWith('/upload/')) {
        const uuid = path.split('/')[2];
        setComponent(<UploadFileList uuid={uuid} />);
      } else {
        setComponent(<div>404 Not Found</div>);
      }
    };

    renderComponentBasedOnPath();

    window.addEventListener('popstate', renderComponentBasedOnPath);

    return () => {
      window.removeEventListener('popstate', renderComponentBasedOnPath);
    };
  }, []);

  return (
    <div>
      {component}
    </div>
  );
};

export default App;
