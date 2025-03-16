import {TestBed} from "@angular/core/testing";
import {DepartmentService} from "@/app/services/departments/department.service";
import {HttpTestingController, provideHttpClientTesting} from "@angular/common/http/testing";
import {provideHttpClient} from "@angular/common/http";
import {DepartmentViewModel} from "@/app/models/departments/department-view-model";

describe('DepartmentService', () => {
  const baseUrl = 'http://localhost:9099';
  let httpTesting: HttpTestingController;
  let service: DepartmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        DepartmentService,
        {provide: 'BASE_URL', useValue: `${baseUrl}/`},
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    httpTesting = TestBed.inject(HttpTestingController);
    service = TestBed.inject(DepartmentService);
  });

  afterEach(() => {
    httpTesting.verify();
  })

  it("getAll should call GET '/api/departments'", () => {
    // Arrange
    const data: DepartmentViewModel[] = [
      {
        id: 1,
        name: 'Test Labs'
      },
      {
        id: 2,
        name: 'Biodome'
      }
    ];

    // Act
    const getAll$ = service.getAll();

    // Assert
    getAll$.subscribe(departments => {
      expect(departments).toEqual(data);
    })
    const req = httpTesting.expectOne(`${baseUrl}/api/departments`, 'Request to load the departments.');
    expect(req.request.method).toBe('GET');
    req.flush(data)
  })
});
