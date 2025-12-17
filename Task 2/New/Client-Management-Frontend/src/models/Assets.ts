export interface Asset {
  id: number;
  clientId: number;
  name: string;
  type: string;
  value: number;
  provider: string;
  asOf: string;
  createdAt: string;
}


export interface AssetSearchParams {
  userId: number;
}