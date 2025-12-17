import { useRef, useState } from "react";
import { useClients } from "../hooks/useClients";
import { Table, Button, Form, Row, Col, Pagination } from 'react-bootstrap';
import { useNavigate } from "react-router-dom";
import type { TableColumn } from "../models/Tables";
import type { ClientInfo } from "../models/Client";



const Clients = () => {
  const columns: TableColumn<ClientInfo>[] = [
    { header: 'ID', accessor: 'id' },
    { header: 'Name', accessor: 'name', isSortable: true },
    { header: 'DOB', accessor: 'dateOfBirth' },
    { header: 'Email', accessor: 'email', isSortable: true },
    { header: 'Phone', accessor: 'phone' },
    { header: 'Created', accessor: 'createdAt', isSortable: true },
  ];

  const navigate = useNavigate();

  const { clientList, setParams, params, goToPage, totalPages, page, clientCount, reset } = useClients({page: 1, pageSize: 10});
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [statusSearch, setStatusSearch] = useState<string>('0');
  const statusRef = useRef<HTMLSelectElement>(null);

  const handleSort = (key: string) => {
    setParams({
      ...params,
      sortBy: key,
      direction: params.sortBy === key && params.direction === 'asc' ? 'desc' : 'asc'
    });
  };

  const handleClear = () => {
    setStatusSearch('0');
    setSearchTerm('');
    reset();
  }

  const handleSearch = () => {
    reset();

    setParams({
      ...params,
      searchTerm: searchTerm,
      status: Number(statusRef.current?.value),
      page: 1
    });
  };

  return (
    <div className="mt-4">
      <h1>Clients</h1>
      
      <Row className="mb-3 g-2">
        <Col md={3}>
          <Form.Control type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} onKeyDown={(e) => e.key === 'Enter' && handleSearch()} placeholder="Search name or email..." />
        </Col>
        <Col md={3}>
          <Form.Select ref={statusRef} value={statusSearch} onChange={(e) => {setStatusSearch(e.target.value); handleSearch()}}>
            <option value="0">All Statuses</option>
            <option value="1">New</option>
            <option value="2">Quotes Sent</option>
            <option value="3">IFA Call Booked</option>
            <option value="4">Application Pack Sent</option>
            <option value="5">Application Pack Back</option>
            <option value="6">Completed</option>
            <option value="7">Not Proceeding</option>
          </Form.Select>
        </Col>
        <Col md="auto">
          <Button variant="primary" onClick={handleSearch}>Search</Button>
        </Col>
        <Col md="auto">
          <Button variant="outline-secondary" onClick={handleClear}>Clear</Button>
        </Col>
        <Col className="text-end">
          <Button variant="success">+ Add Client</Button>
        </Col>
      </Row>

      <Table hover responsive className="border-top">
        <thead className="bg-light">
          <tr>
            {columns.map((col) => (<th className={col.isSortable ? "sortable" : ""} onClick={() => col.isSortable && handleSort(col.accessor)} style={col.isSortable ? {cursor: "pointer", color: "blue", textDecoration: "underline"} : {}}>{col.header}</th>))}
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {clientList.map((client) => (
            <tr key={client.id} onClick={() => navigate(`/clients/${client.id}`)} style={{cursor: "pointer"}}>
              {columns.map((col) => (
                <td>{client[col.accessor]}</td>
              ))}
              <td>
                <Button variant="link" className="text-danger p-0">Delete</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <div className="d-flex justify-content-between align-items-center mt-3">
        <span className="text-muted">
          Showing page <strong>{page}</strong> of <strong>{totalPages}</strong> ({clientCount} total clients)
        </span>

        <Pagination className="mb-0">
          <Pagination.First onClick={() => goToPage(1)} disabled={page === 1} />
          <Pagination.Prev onClick={() => goToPage(page - 1)} disabled={page === 1} />

          {Array.from({ length: totalPages }, (_, i) => (
            <Pagination.Item key={i + 1} active={i + 1 === page} onClick={() => goToPage(i + 1)}>
              {i + 1}
            </Pagination.Item>
          ))}

          <Pagination.Next onClick={() => goToPage(page + 1)} disabled={page === totalPages} />
          <Pagination.Last onClick={() => goToPage(totalPages)} disabled={page === totalPages} />
        </Pagination>
      </div>
    </div>
  );
};

export default Clients;