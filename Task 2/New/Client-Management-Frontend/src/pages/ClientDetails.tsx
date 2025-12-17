import { Button, Container, Spinner } from "react-bootstrap";
import { useParams } from "react-router-dom";
import ClientDetailsCard from "../components/Client/ClientDetailsCard";
import InformationCard, { type EntryField } from "../components/Client/InformationCard";
import useClientDetails from "../hooks/useClientDetails";
import type { Asset } from "../models/Assets";
import type { Liability } from "../models/Liabilities";
import type { TableColumn } from "../models/Tables";
import type { Journal } from "../models/Journal";
import type { Income } from "../models/Income";
import type { Expenditure } from "../models/Expenditure";



const ClientDetails = () => {
  const { id } = useParams<{ id: string }>();

  const {client, loading} = useClientDetails(id);

  const journalColumns: TableColumn<Journal>[] = [
    { header: 'ID', accessor: 'id' },
    { header: 'Occurred', accessor: 'occurredAt' },
    { header: 'Author', accessor: 'author' },
    { header: 'Body', accessor: 'body' }
  ];

  const assetColumns: TableColumn<Asset>[] = [
    { header: 'ID', accessor: 'id' },
    { header: 'Type', accessor: 'type' },
    { header: 'Value', accessor: 'value' },
    { header: 'Provider', accessor: 'provider' },
    { header: 'As Of', accessor: 'asOf' }
  ]

  const liabilityColumns: TableColumn<Liability>[] = [
    { header: 'ID', accessor: 'id' },
    { header: 'Type', accessor: 'type' }, 
    { header: 'Balance', accessor: 'balance' },
    { header: 'Rate', accessor: 'rate' },
    { header: 'As Of', accessor: 'asOf' }
  ];

  const incomeColumns: TableColumn<Income>[] = [
    { header: 'ID', accessor: 'id' },
    { header: 'Source', accessor: 'source' },
    { header: 'Amount', accessor: 'amountMonthly' },
    { header: 'As Of', accessor: 'asOf' }
  ];

  const expenditureColumns: TableColumn<Expenditure>[] = [
    { header: 'ID', accessor: 'id' },
    { header: 'Category', accessor: 'category' },
    { header: 'Amount', accessor: 'amountMonthly' },
    { header: 'As Of', accessor: 'asOf' }
  ];




  const journalEntryFields: EntryField[] = [
    { accessor: 'occured', label: 'Occurred', type: 'date' },
    { accessor: 'author', label: 'Author', type: 'text' },
    { accessor: 'type', label: 'Note Type', type: 'dropdown', options: [
      { label: 'General', value: 'general' },
      { label: 'Reminder', value: 'reminder' },
      { label: 'Alert', value: 'alert' }
    ]},
    { accessor: 'body', label: 'Body', type: 'richtext' }
  ];

  const assetEntryFields: EntryField[] = [
    { accessor: 'type', label: 'Asset Type', type: 'text' },
    { accessor: 'value', label: 'Value', type: 'number' },
    { accessor: 'provider', label: 'Provider', type: 'text' }
  ];
  const liabilityEntryFields: EntryField[] = [
    { accessor: 'type', label: 'Type', type: 'text' },
    { accessor: 'balance', label: 'Balance', type: 'number' },
    { accessor: 'rate', label: 'Rate', type: 'number' }
  ];
  const incomeEntryFields: EntryField[] = [
    { accessor: 'source', label: 'Source', type: 'text' },
    { accessor: 'amountMonthly', label: 'Amount Monthly', type:'number' }
  ];
  const expenditureEntryFields : EntryField[] = [
    {accessor:'category', label:'Category', type:'text' },
    {accessor:'amountMonthly', label:'Amount Monthly', type:'number' }
  ];


  if (loading) return <Spinner animation="border" className="m-5" />;
  if (!client) return <div>Client not found.</div>;


  return (
    <Container className="mt-4">
      
      <Button variant="secondary" href="/clients" className="mb-3">Back to Clients</Button>

      <ClientDetailsCard client={client}></ClientDetailsCard>
      <InformationCard title="Journals" items={client.journals || []} tableColumns={journalColumns || []} entryFields={journalEntryFields}></InformationCard>
      <InformationCard title="Assets" items={client.assets || []} tableColumns={assetColumns || []} entryFields={assetEntryFields}></InformationCard>
      <InformationCard title="Liabilities" items={client.liabilities || []} tableColumns={liabilityColumns || []} entryFields={liabilityEntryFields}></InformationCard>
      <InformationCard title="Income" items={client.incomes || []} tableColumns={incomeColumns || []} entryFields={incomeEntryFields}></InformationCard>
      <InformationCard title="Expenditure" items={client.expenditures || []} tableColumns={expenditureColumns || []} entryFields={expenditureEntryFields}></InformationCard>
      
    </Container>
  );
};

export default ClientDetails;