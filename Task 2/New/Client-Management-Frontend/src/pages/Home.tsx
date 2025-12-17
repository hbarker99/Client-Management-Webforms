import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";



const Home = () => {

  const navigate = useNavigate();

  return (
    <div className="p-5">
      <h1>Client Management Demo</h1>
      <p>Simple client management with journals, assets, liabilities, income and expenditure.</p>
      <Button variant="primary" onClick={() => navigate("/clients")}>Go To Clients {'>>'}</Button>
      <hr />
    </div>
  );

};

export default Home;