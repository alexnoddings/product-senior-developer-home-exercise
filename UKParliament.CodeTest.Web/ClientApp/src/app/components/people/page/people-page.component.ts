import {Component, inject, OnInit} from '@angular/core';
import {PeopleTableComponent} from "@/app/components/people/table/people-table.component";
import {PersonEditorEmptyComponent} from "@/app/components/people/editor/empty/person-editor-empty.component";
import {PersonEditorNewComponent} from "@/app/components/people/editor/new/person-editor-new.component";
import {PersonEditorExistingComponent} from "@/app/components/people/editor/existing/person-editor-existing";
import {Subject} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-people-page',
  standalone: true,
  imports: [
    PeopleTableComponent,
    PersonEditorEmptyComponent,
    PersonEditorNewComponent,
    PersonEditorExistingComponent
  ],
  templateUrl: './people-page.component.html'
})
export class PeoplePageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  peopleUpdated = new Subject<void>();
  selectedPersonId: number | 'new' | undefined;

  ngOnInit(): void {
    // Not a spec requirement, but SUPER helpful for me while developing and the page keeps reloading!
    this.route.queryParams.subscribe(params => {
      const queryPerson = params.person;
      if (queryPerson == undefined) {
        this.selectedPersonId = undefined;
      } else if (queryPerson == 'new') {
        this.selectedPersonId = 'new';
      } else {
        const personId = parseInt(queryPerson);
        if (!isNaN(personId)) {
          // Bypass calling selectPerson since the query param is already set
          this.selectedPersonId = queryPerson;
        }
      }
    });
  }

  refreshPeople(): void {
    this.peopleUpdated.next();
  }

  selectPerson(id: number | 'new' | undefined): void {
    if (id != this.selectedPersonId) {
      this.selectedPersonId = id;
      this.router.navigate([], {queryParams: {person: id}, relativeTo: this.route});
    }
  }

  onPersonAdded(id: number): void {
    this.selectPerson(id);
    this.refreshPeople();
  }

  onPersonUpdated(_: number): void {
    this.selectPerson(undefined);
    this.refreshPeople();
  }

  onPersonAddEditCancelled(): void {
    this.selectPerson(undefined);
  }
}
