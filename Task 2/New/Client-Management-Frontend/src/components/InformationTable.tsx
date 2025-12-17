import { Button, Table } from "react-bootstrap";
import type { TableColumn } from "../models/Tables";

export interface InformationTableProps<T> {
  items: T[];
  columns: TableColumn<T>[];
}

function InformationTable<T extends { id: number }>({ items, columns }: InformationTableProps<T>) {
  return (
      <Table responsive className="border-top">
        <thead className="bg-light">
          <tr>
            {columns.map((col) => (<th>{col.header}</th>))}
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <tr key={item.id}>
              {columns.map((col) => (
                <td key={String(col.accessor)}>{item[col.accessor] as string}</td>
              ))}
              <td>
                <Button variant="link" className="text-danger p-0">Delete</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
  );
}

export default InformationTable;