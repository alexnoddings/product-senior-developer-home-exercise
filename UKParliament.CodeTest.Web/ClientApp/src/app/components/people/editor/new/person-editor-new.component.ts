import {Component, inject, OnInit, output} from '@angular/core';
import {PeopleService} from "@/app/services/people/people.service";
import {PersonEditModel} from "@/app/components/people/editor/form/person-editor-form.model";
import {ProblemDetails} from "@/app/models/problem-details";
import {PersonEditorFormComponent} from "@/app/components/people/editor/form/person-editor-form.component";
import {ErrorsOutletComponent} from "@/app/components/errors/errors-outlet.component";

@Component({
  selector: 'app-person-editor-new',
  standalone: true,
  imports: [
    PersonEditorFormComponent,
    ErrorsOutletComponent
  ],
  templateUrl: './person-editor-new.component.html'
})
export class PersonEditorNewComponent implements OnInit {
  private peopleService = inject(PeopleService);

  saved = output<number>();
  cancelled = output<void>();

  person: PersonEditModel | undefined;
  error?: ProblemDetails;

  ngOnInit(): void {
    this.loadPerson();
  }

  private loadPerson(): void {
    this.person = {
      firstName: '',
      lastName: '',
      email: '',
    };
  }

  cancel(): void {
    this.cancelled.emit();
  }

  save(newPerson: PersonEditModel): void {
    const model = {
      firstName: newPerson.firstName!,
      lastName: newPerson.lastName!,
      email: newPerson.email!,
      dateOfBirth: newPerson.dateOfBirth!,
      departmentId: newPerson.departmentId!,
    };

    this.peopleService
      .create(model)
      .subscribe({
        next: personId => {
          this.saved.emit(personId);
        },
        error: err => {
          // Assumes the error was that the server responded with non-2xx,
          // and that said response contains a ProblemDetails.
          // Won't work if the request doesn't connect,
          // or if the server doesn't return a ProblemDetails.
          this.error = err.error;
        }
      });
  }
}
