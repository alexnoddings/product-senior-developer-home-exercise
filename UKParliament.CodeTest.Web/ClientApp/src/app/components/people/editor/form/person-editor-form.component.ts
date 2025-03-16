import {Component, inject, input, OnChanges, OnInit, output, SimpleChanges} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {AsyncPipe} from "@angular/common";
import {PersonEditorEmptyComponent} from "@/app/components/people/editor/empty/person-editor-empty.component";
import {DepartmentService} from "@/app/services/departments/department.service";
import {PersonEditModel} from "@/app/components/people/editor/form/person-editor-form.model";
import {Observable} from "rxjs";
import {DepartmentViewModel} from "@/app/models/departments/department-view-model";

@Component({
  selector: 'app-person-editor-form',
  standalone: true,
  imports: [ReactiveFormsModule, AsyncPipe, PersonEditorEmptyComponent],
  templateUrl: './person-editor-form.component.html',
  styleUrls: ['./person-editor-form.component.scss']
})
export class PersonEditorFormComponent implements OnInit, OnChanges {
  private departmentService = inject(DepartmentService);
  // This doesn't have any kind of caching, so will fetch the department list every time the component is loaded.
  departments$?: Observable<DepartmentViewModel[]>;

  // Passes a person model in for editing
  person = input<PersonEditModel>();
  saved = output<PersonEditModel>();
  cancelled = output<void>();

  personForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.maxLength(100)]),
    lastName: new FormControl('', [Validators.required, Validators.maxLength(100)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    // Doesn't validate min/max values, relies on server for that
    // Could be added with a custom validator, Validators.min/max only support numerics
    dateOfBirth: new FormControl<Date | undefined>(undefined, [Validators.required]),
    departmentId: new FormControl<number | undefined>(undefined, [Validators.required])
  });

  ngOnInit(): void {
    this.loadDepartments();
    this.loadPerson();
  }

  // updates the person if they change (eg switching between people from the table)
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.person != undefined) {
      this.loadPerson();
    }
  }

  loadDepartments(): void {
    this.departments$ = this.departmentService.getAll();
  }

  loadPerson(): void {
    const person = this.person();
    if (person != undefined) {
      this.personForm.setValue({
        firstName: person.firstName,
        lastName: person.lastName,
        email: person.email,
        dateOfBirth: person.dateOfBirth ?? null,
        departmentId: person.departmentId ?? null
      });
    }
  }

  cancel(): void {
    this.cancelled.emit();
  }

  save(): void {
    const person = this.person();
    if (person != undefined) {
      // Assume all values are set here as the validator shouldn't let undefined values through
      const form = this.personForm.value;
      const model: PersonEditModel = {
        firstName: form.firstName!,
        lastName: form.lastName!,
        email: form.email!,
        dateOfBirth: form.dateOfBirth!,
        departmentId: form.departmentId!,
      };
      this.saved.emit(model);
    }
  }
}
