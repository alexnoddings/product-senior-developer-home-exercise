import {ErrorsOutletComponent} from "@/app/components/errors/errors-outlet.component";
import {ComponentFixture, TestBed} from "@angular/core/testing";
import {ProblemDetails } from "@/app/models/problem-details";

describe('ErrorsOutletComponent', () => {
  let component: ErrorsOutletComponent;
  let fixture: ComponentFixture<ErrorsOutletComponent>;

  const error: ProblemDetails = {
    title: 'an error title',
    status: 400,
    detail: 'an error detail',
    errors: {
      ['PropertyA']: [
        'PropertyA error #1',
        'PropertyA error #2'
      ]
    }
  };

  beforeEach(async () => {
    TestBed.configureTestingModule({imports: [ErrorsOutletComponent]});
    fixture = TestBed.createComponent(ErrorsOutletComponent);
    component = fixture.componentInstance;

    fixture.componentRef.setInput('error', error);
    fixture.detectChanges();
  });

  it('should show the error messages', () => {
    expect(fixture.nativeElement.textContent).toContain('an error title');
    expect(fixture.nativeElement.textContent).toContain('an error detail');
    expect(fixture.nativeElement.textContent).toContain('PropertyA error #1');
    expect(fixture.nativeElement.textContent).toContain('PropertyA error #2');
  })

});
