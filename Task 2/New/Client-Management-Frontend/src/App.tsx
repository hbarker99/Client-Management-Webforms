import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Container } from 'react-bootstrap';
import Header from './components/Header';
import Home from './pages/Home';
import Clients from './pages/Clients';
import ClientDetails from './pages/ClientDetails';

// Simple placeholder components for your pages
const Dashboard = () => <div className="p-5"><h1>Dashboard</h1><p>Stats and charts go here.</p></div>;

function App() {
  return (
    <Router>
      <Header />
      <Container className="mt-4">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/clients" element={<Clients />} />
          <Route path="/clients/:id" element={<ClientDetails />} />
          <Route path="/dashboard" element={<Dashboard />} />
        </Routes>
      </Container>
    </Router>
  );
}

export default App;