import {TestBed} from "@angular/core/testing";
import {HttpTestingController, provideHttpClientTesting} from "@angular/common/http/testing";
import {provideHttpClient} from "@angular/common/http";
import {PeopleService} from "@/app/services/people/people.service";
import {PersonViewModel} from "@/app/models/people/person-view-model";

describe('PeopleService', () => {
  const baseUrl = 'http://localhost:9099';
  let httpTesting: HttpTestingController;
  let service: PeopleService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PeopleService,
        {provide: 'BASE_URL', useValue: `${baseUrl}/`},
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    httpTesting = TestBed.inject(HttpTestingController);
    service = TestBed.inject(PeopleService);
  });

  afterEach(() => {
    httpTesting.verify();
  })

  // For brevity/time, I'm only testing a couple of methods as examples
  it("getById should call GET '/api/people/:id'", () => {
    // Arrange
    const data: PersonViewModel =
      {
        id: 42,
        firstName: 'Jeff',
        lastName: 'Mangum',
        email: 'jeff@neutralmilk.hotel',
        dateOfBirth: new Date("1980-01-02"),
        department: {
          id: 2,
          name: 'entertainment'
        }
      };

    // Act
    const getById$ = service.getById(42);

    // Assert
    getById$.subscribe(person => {
      expect(person).toEqual(data);
    })
    const req = httpTesting.expectOne(`${baseUrl}/api/people/42`, 'Request to load the person.');
    expect(req.request.method).toBe('GET');
    req.flush(data)
  })

  it("delete should call DELETE '/api/people/:id'", () => {
    // Act
    const delete$ = service.delete(42);

    // Assert
    delete$.subscribe(wasDeleted => {
      expect(wasDeleted);
    })
    const req = httpTesting.expectOne(`${baseUrl}/api/people/42`, 'Request to delete the person.');
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  })
});
