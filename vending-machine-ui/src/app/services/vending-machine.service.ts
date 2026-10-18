import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VendingMachine, Shelf, ProductType } from '../models/vending-machine.model';

@Injectable({
  providedIn: 'root'
})
export class VendingMachineService {
  private apiUrl = 'https://localhost:7035/api/vending-machines';

  constructor(private http: HttpClient) {}

  createMachine(name: string, location: string, maxShelves: number): Observable<VendingMachine> {
    return this.http.post<VendingMachine>(this.apiUrl, { name, location, maxShelves });
  }

  getMachineById(id: string): Observable<VendingMachine> {
    return this.http.get<VendingMachine>(`${this.apiUrl}/${id}`);
  }

  addShelf(machineId: string, productType: ProductType, maxCapacity: number): Observable<VendingMachine> {
    return this.http.post<VendingMachine>(`${this.apiUrl}/${machineId}/shelves`, { productType, maxCapacity });
  }

  loadInventory(machineId: string, shelfId: string, productType: ProductType, quantity: number): Observable<VendingMachine> {
    return this.http.post<VendingMachine>(`${this.apiUrl}/${machineId}/shelves/${shelfId}/inventory`, { productType, quantity });
  }

  dispenseProduct(machineId: string, shelfId: string): Observable<VendingMachine> {
    return this.http.post<VendingMachine>(`${this.apiUrl}/${machineId}/shelves/${shelfId}/dispense`, {});
  }

  getAllMachines(): Observable<VendingMachine[]> {
  return this.http.get<VendingMachine[]>(this.apiUrl);
}
}