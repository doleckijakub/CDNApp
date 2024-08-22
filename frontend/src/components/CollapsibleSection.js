import React, { useState } from 'react';
import './CollapsibleSection.css'

const CollapsibleSection = ({ title, children }) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className="collapsible-section">
            <h4 onClick={() => setIsOpen(!isOpen)} className="collapsible-title">
                <span>{title}</span>
                <span className="collapsible-arrow">{isOpen ? '▲' : '▼'}</span>
            </h4>
            {isOpen && <div className="collapsible-content">{children}</div>}
        </div>
    );
};

export default CollapsibleSection;