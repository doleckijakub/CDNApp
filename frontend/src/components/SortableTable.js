import React, { useState } from 'react';

const SortableTable = ({ data }) => {
  const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });

  const headers = data && data.length > 0 ? Object.keys(data[0]) : [];

  const sortedData = React.useMemo(() => {
    let sortableItems = [...data];
    if (sortConfig !== null) {
      sortableItems.sort((a, b) => {
        if (a[sortConfig.key] < b[sortConfig.key]) {
          return sortConfig.direction === 'ascending' ? -1 : 1;
        }
        if (a[sortConfig.key] > b[sortConfig.key]) {
          return sortConfig.direction === 'ascending' ? 1 : -1;
        }
        return 0;
      });
    }
    return sortableItems;
  }, [data, sortConfig]);

  const requestSort = (key) => {
    let direction = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  const getClassNamesFor = (key) => {
    if (!sortConfig) {
      return;
    }
    return sortConfig.key === key ? sortConfig.direction : undefined;
  };

  if (headers.length === 0) {
    return <p>No data available to display</p>;
  }

  return (
    <table class="table">
      <thead>
        <tr>
          {headers.map((key) => (
            <th
              key={key}
              onClick={() => requestSort(key)}
              className={getClassNamesFor(key)}
              class="table-header"
            >
              {key.charAt(0).toUpperCase() + key.slice(1)}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {sortedData.map((item, index) => (
          <tr key={index}>
            {headers.map((key) => (
              <td key={key}>{item[key]}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default SortableTable;
