import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceRequestMaintenanceComponent } from './service-request-maintenance.component';

describe('ServiceRequestMaintenanceComponent', () => {
  let component: ServiceRequestMaintenanceComponent;
  let fixture: ComponentFixture<ServiceRequestMaintenanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ServiceRequestMaintenanceComponent]
    });
    fixture = TestBed.createComponent(ServiceRequestMaintenanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
