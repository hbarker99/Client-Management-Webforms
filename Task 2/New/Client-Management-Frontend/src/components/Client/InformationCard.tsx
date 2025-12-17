import type { TableColumn } from "../../models/Tables";
import InformationTable from "../InformationTable";
import { useState } from "react";
import Row from "react-bootstrap/esm/Row";
import Col from "react-bootstrap/esm/Col";
import { Button, Form } from "react-bootstrap";


interface InformationCardProps<T extends {id: number}> {
  title: string;
  entryFields: EntryField[];
  tableColumns: TableColumn<T>[];
  items: T[];
}

export type EntryFieldType = 'text' | 'number' | 'dropdown' | 'richtext' | 'date';

export interface EntryField {
  accessor: string;
  label: string;
  type: EntryFieldType;
  options?: { label: string; value: string | number }[];
}

function TableRender<T extends {id: number}>(items: T[], tableColumns: TableColumn<T>[]) {
  if (items.length === 0) {
    return (<div>No Items Available.</div>)
  }
  return <InformationTable items={items} columns={tableColumns} />;
}

function InformationCard<T extends {id: number}>({ title, entryFields, tableColumns, items }: InformationCardProps<T>) {
  
  const [newEntry, setNewEntry] = useState<Record<string, string>>({});

  const handleInputChange = (accessor: string, value: string) => {
    setNewEntry(prev => ({ ...prev, [accessor]: value }));
  };

  const handleAdd = () => {
    console.log(`Adding new ${title}:`, newEntry);
    setNewEntry({});
  };


  return (
    <div className="card mb-3">
      <div className="card-header">
        <h5 className="mb-0">{title}</h5>
      </div>
      <div className="card-body">
        <Form className="mb-4 pb-4 border-bottom">
          <Row className="g-3 align-items-end">
            {entryFields.map((field) => (
              <Col key={field.accessor} md={field.type === 'richtext' ? 12 : 3}>
                <Form.Label className="small fw-bold text-muted text-uppercase">
                  {field.label}
                </Form.Label>

                {field.type === 'dropdown' ? (
                  <Form.Select 
                    value={newEntry[field.accessor] || ''}
                    onChange={(e) => handleInputChange(field.accessor, e.target.value)}
                  >
                    <option value="">Select...</option>
                    {field.options?.map(opt => (
                      <option key={opt.value} value={opt.value}>{opt.label}</option>
                    ))}
                  </Form.Select>
                ) : field.type === 'number' ? (
                  <Form.Control 
                    type="number"
                    step="0.01" // Ensures 2 decimal places
                    value={newEntry[field.accessor] || ''}
                    onChange={(e) => handleInputChange(field.accessor, e.target.value)}
                  />
                ) : field.type === 'richtext' ? (
                  <Form.Control 
                    as="textarea" 
                    rows={3} 
                    value={newEntry[field.accessor] || ''}
                    onChange={(e) => handleInputChange(field.accessor, e.target.value)}
                  />
                ) : field.type === 'text' ? (
                  <Form.Control 
                    type="text"
                    value={newEntry[field.accessor] || ''}
                    onChange={(e) => handleInputChange(field.accessor, e.target.value)}
                  />
                ) : (
                  <Form.Control 
                    type="date"
                    value={newEntry[field.accessor] || ''}
                    onChange={(e) => handleInputChange(field.accessor, e.target.value)}
                  />
                )}
              </Col>
            ))}
            
            <Col md="12" align="end">
              <Button variant="success" onClick={handleAdd} className="px-4 shadow-sm">
                + Add
              </Button>
            </Col>
          </Row>
        </Form>
        {TableRender(items || [], tableColumns)}

      </div>
    </div>
  );
};

export default InformationCard;