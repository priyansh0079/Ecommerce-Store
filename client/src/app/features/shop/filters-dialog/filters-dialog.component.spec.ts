import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FiltersDialogComponent } from './filters-dialog.component';

describe('FiltersDialogComponent', () => {
  let component: FiltersDialogComponent;
  let fixture: ComponentFixture<FiltersDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FiltersDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FiltersDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
