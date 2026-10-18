import { Component } from '@angular/core';
import { VendingMachineService } from '../../services/vending-machine.service';
import { VendingMachine, ProductType } from '../../models/vending-machine.model';

@Component({
  selector: 'app-vending-dashboard',
  templateUrl: './vending-dashboard.component.html',
  styleUrls: ['./vending-dashboard.component.css']
})
export class VendingDashboardComponent {
  searchId = '';
  currentMachine: VendingMachine | null = null;
  errorMessage = '';

  // טופס יצירת מכונה
  newMachineName = '';
  newMachineLocation = '';
  newMachineMaxShelves = 5;

  // טופס הוספת מדף
  shelfProductType: ProductType = ProductType.Cola;
  shelfMaxCapacity = 10;

  // טופס טעינת מלאי
  inventoryQuantity = 5;

  //טופס שליפת כל המכונות
  allMachines: VendingMachine[] = [];

  constructor(private vmService: VendingMachineService) {}

  // 1. חיפוש מכונה לפי ID
  searchMachine() {
    if (!this.searchId) return;
    this.errorMessage = '';
    this.vmService.getMachineById(this.searchId).subscribe({
      next: (machine) => {
        this.currentMachine = machine;
      },
      error: (err) => {
        if (err.status === 400 || err.status === 404) {
          alert('שגיאה: המכונה לא נמצאה או שהמזהה שגוי.');
        }
      }
    });
  }

  // 2. יצירת מכונה חדשה
createMachine() {
    console.log('מנסה ליצור מכונה עם ערכים:', {
      name: this.newMachineName,
      location: this.newMachineLocation,
      maxShelves: this.newMachineMaxShelves
    });

    if (!this.newMachineName) {
      alert('חובה להזין שם מכונה ומספר מדפים מקסימלי');
      return;
    }

    this.vmService.createMachine(this.newMachineName, this.newMachineLocation, this.newMachineMaxShelves).subscribe({
      next: (machine) => {
        console.log('השרת החזיר הצלחה:', machine);
        alert('המכונה נוצרה בהצלחה!');
        this.currentMachine = machine;
        this.newMachineName = '';
        this.newMachineLocation = '';
        this.newMachineLocation = '';
      },
      error: (err) => {
        console.error('שגיאה בתקשורת עם השרת:', err);
        alert('שגיאה בשרת: ' + (err.status || 'לא ידוע'));
      }
    });
  }

  // 3. הוספת מדף
  addShelf(machineId: string) {
    this.vmService.addShelf(machineId, Number(this.shelfProductType), this.shelfMaxCapacity).subscribe({
      next: (machine) => {
        this.currentMachine = machine;
        alert('המדף נוסף בהצלחה!');
      },
      error: (err) => {
        if (err.status === 400) {
          alert('לא ניתן להוסיף מדף: חרגת מכמות המדפים המקסימלית שהוגדרה למכונה!');
        }
      }
    });
  }

  // 4. טעינת מלאי למדף
loadInventory(machineId: string, shelfId: string, productType: ProductType, quantity: number) {
  this.vmService.loadInventory(machineId, shelfId, productType, quantity).subscribe({
    next: (updatedMachine) => {
      alert('המלאי נטען בהצלחה!');
      this.currentMachine = updatedMachine; // מעדכן את המכונה עם הנתונים החדשים מהשרת
    },
    error: (err) => {
      console.error('שגיאה בטעינת מלאי:', err);
      alert('שגיאה בטעינת המלאי');
    }
  });
}

  // 5. רכישת מוצר
  dispense(machineId: string, shelfId: string) {
    this.vmService.dispenseProduct(machineId, shelfId).subscribe({
      next: (machine) => {
        this.currentMachine = machine;
        alert('המוצר נרכש בהצלחה!');
      },
      error: (err) => {
        if (err.status === 400) {
          alert('שגיאה ברכישת המוצר (ודא שקיימת כמות במדף)');
        }
      }
    });
  }

//6. שליפת כל המכונות
  loadAllMachines() {
  this.vmService.getAllMachines().subscribe({
    next: (machines) => {
      this.allMachines = machines;
    },
    error: (err) => {
      // שגיאות 500 מטופלות כבר באינטרספטור הגלובלי, כאן נטפל ב-400 אם צריך
      if (err.status === 400) {
        alert('שגיאה בטעינת רשימת המכונות');
      }
    }
  });
}
}