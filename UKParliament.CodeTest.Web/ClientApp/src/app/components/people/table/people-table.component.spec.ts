import {ComponentFixture, fakeAsync, TestBed, tick} from "@angular/core/testing";
import {PeopleTableComponent} from "@/app/components/people/table/people-table.component";
import {PeopleService} from "@/app/services/people/people.service";
import {PersonViewModel} from "@/app/models/people/person-view-model";
import {defer} from "rxjs";
import {provideHttpClient} from "@angular/common/http";
import {provideHttpClientTesting} from "@angular/common/http/testing";

describe('PeopleTableComponent', () => {
  let peopleService: PeopleService;
  let getAllSpy: jasmine.Spy;

  let component: PeopleTableComponent;
  let fixture: ComponentFixture<PeopleTableComponent>;

  const data: PersonViewModel[] = [
    {
      id: 101,
      firstName: 'David',
      lastName: 'Bowie',
      email: 'ziggy@stardust.com',
      dateOfBirth: new Date("1980-01-02"),
      department: {
        id: 2,
        name: 'entertainment'
      }
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [PeopleTableComponent],
      providers: [
        PeopleService,
        {provide: 'BASE_URL', useValue: `/`},
        provideHttpClient(),
        provideHttpClientTesting()
      ],
    });

    peopleService = TestBed.inject(PeopleService);
    getAllSpy = spyOn(peopleService, 'getAll')
      .and.returnValue(
        defer(() => Promise.resolve(data))
      );

    fixture = TestBed.createComponent(PeopleTableComponent);
    component = fixture.componentInstance;
  });

  describe('when loading data', () => {
    it('should show loading text', () => {
      fixture.detectChanges();
      expect(fixture.nativeElement.textContent).toContain('Loading');
    });

    it('should show person details once loaded', fakeAsync(() => {
      fixture.detectChanges();
      tick();
      fixture.detectChanges();
      expect(fixture.nativeElement.textContent).toContain('David Bowie');
    }));
  })

});
