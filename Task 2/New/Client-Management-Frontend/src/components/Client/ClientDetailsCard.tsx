import { Button, Card, Col, Form, Row } from "react-bootstrap";
import type { ClientInfo } from "../../models/Client";
import { useState } from "react";



const ClientDetailsCard = ({ client }: { client: ClientInfo }) => {
  const [formData, setFormData] = useState<Partial<ClientInfo>>(client);
  
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSave = async () => {
    console.log("Saving data:", formData);
  };

  return (
    <Card className="mb-3">
        <Card.Header>
          <h3>Client Details - {client.name}</h3>
        </Card.Header>
        <Card.Body>
          <Form>
            <Row className="mb-3">
              <Col md={6}>
                <Form.Group controlId="firstName">
                  <Form.Label className="small fw-bold">First name</Form.Label>
                  <Form.Control 
                    type="text" 
                    name="firstName" 
                    defaultValue={formData.firstName} 
                    onChange={() => handleChange} 
                  />
                </Form.Group>
              </Col>
              <Col md={6}>
                <Form.Group controlId="lastName">
                  <Form.Label className="small fw-bold">Last name</Form.Label>
                  <Form.Control 
                    type="text" 
                    name="lastName" 
                    defaultValue={formData.lastName} 
                    onChange={() => handleChange} 
                  />
                </Form.Group>
              </Col>
            </Row>

            {/* Row 2: DOB, Email, Phone */}
            <Row className="mb-3">
              <Col md={4}>
                <Form.Group controlId="dob">
                  <Form.Label className="small fw-bold">Date of birth</Form.Label>
                  <Form.Control 
                    type="date" 
                    name="dateOfBirth" 
                    value={formData.dateOfBirth?.toString()} 
                    onChange={() => handleChange} 
                  />
                </Form.Group>
              </Col>
              <Col md={4}>
                <Form.Group controlId="email">
                  <Form.Label className="small fw-bold">Email</Form.Label>
                  <Form.Control 
                    type="email" 
                    name="email" 
                    value={formData.email} 
                    onChange={() => handleChange} 
                  />
                </Form.Group>
              </Col>
              <Col md={4}>
                <Form.Group controlId="phone">
                  <Form.Label className="small fw-bold">Phone</Form.Label>
                  <Form.Control 
                    type="text" 
                    name="phone" 
                    value={formData.phone} 
                    onChange={() => handleChange} 
                  />
                </Form.Group>
              </Col>
            </Row>

            {/* Row 3: Status */}
            <Row className="mb-4">
              <Col md={4}>
                <Form.Group controlId="status">
                  <Form.Label className="small fw-bold">Status</Form.Label>
                  <Form.Select 
                    name="status" 
                    value={formData.status} 
                    onChange={() => handleChange}
                  >
                    <option value="1">New</option>
                    <option value="2">Application Pack Sent</option>
                    <option value="6">Completed</option>
                    {/* ... other options */}
                  </Form.Select>
                </Form.Group>
              </Col>
            </Row>

            {/* Actions */}
            <div className="d-flex gap-2 border-top pt-3 justify-content-end">
              <Button variant="primary" onClick={handleSave} className="px-4">
                Save
              </Button>
              <Button variant="danger" className="px-4">
                Delete
              </Button>
            </div>
          </Form>
        </Card.Body>
      </Card>


  );
}

export default ClientDetailsCard;