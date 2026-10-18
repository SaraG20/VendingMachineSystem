import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendingDashboardComponent } from './vending-dashboard.component';

describe('VendingDashboardComponent', () => {
  let component: VendingDashboardComponent;
  let fixture: ComponentFixture<VendingDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [VendingDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VendingDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
