import {Component, inject, input, OnChanges, OnInit, output, SimpleChanges} from '@angular/core';
import {PeopleService} from "@/app/services/people/people.service";
import {PersonEditorFormComponent} from "@/app/components/people/editor/form/person-editor-form.component";
import {ErrorsOutletComponent} from "@/app/components/errors/errors-outlet.component";
import {PersonEditModel} from "@/app/components/people/editor/form/person-editor-form.model";
import {ProblemDetails} from "@/app/models/problem-details";
import {PersonEditorEmptyComponent} from "@/app/components/people/editor/empty/person-editor-empty.component";

@Component({
  selector: 'app-person-editor-existing',
  standalone: true,
  imports: [
    PersonEditorFormComponent,
    ErrorsOutletComponent,
    PersonEditorEmptyComponent
  ],
  templateUrl: './person-editor-existing.component.html',
})
export class PersonEditorExistingComponent implements OnInit, OnChanges {
  private peopleService = inject(PeopleService);

  personId = input.required<number>();
  saved = output<number>();
  cancelled = output<void>();

  person: PersonEditModel | undefined;
  error?: ProblemDetails;

  ngOnInit(): void {
    this.loadPerson();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.personId != undefined) {
      this.loadPerson();
    }
  }

  private loadPerson(): void {
    const personId = this.personId();

    this.error = undefined;
    this.peopleService
      .getById(personId)
      .subscribe({
        next: person => {
          this.person = {
            firstName: person.firstName,
            lastName: person.lastName,
            email: person.email,
            dateOfBirth: person.dateOfBirth,
            departmentId: person.department.id
          };
        },
        error: err => {
          if (err.status == 404) {
            this.error = {
              title: 'Person was deleted, or does not exist.',
              status: 404
            };
          } else {
            // Assumes the error was that the server responded with non-2xx,
            // and that said response contains a ProblemDetails.
            // Won't work if the request doesn't connect,
            // or if the server doesn't return a ProblemDetails.
            this.error = err.error;
          }
        }
      });
  }

  cancel(): void {
    this.cancelled.emit();
  }

  save(newPerson: PersonEditModel): void {
    const personId = this.personId();
    const model = {
      firstName: newPerson.firstName!,
      lastName: newPerson.lastName!,
      email: newPerson.email!,
      dateOfBirth: newPerson.dateOfBirth!,
      departmentId: newPerson.departmentId!,
    };

    this.peopleService
      .update(personId, model)
      .subscribe({
        next: () => {
          this.saved.emit(personId);
        },
        error: err => {
          if (err.status == 404) {
            this.error = {
              title: 'Person was deleted.',
              status: 404
            };
          } else {
            this.error = err.error;
          }
        }
      });
  }
}
