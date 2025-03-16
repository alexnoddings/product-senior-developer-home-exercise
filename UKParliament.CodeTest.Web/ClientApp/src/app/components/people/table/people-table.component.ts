import {Component, inject, input, model, OnDestroy, OnInit, output} from '@angular/core';
import {PeopleService} from "@/app/services/people/people.service";
import {Observable, Subscription} from "rxjs";
import {PersonViewModel} from "@/app/models/people/person-view-model";
import {AsyncPipe} from "@angular/common";

@Component({
  selector: 'app-people-table',
  standalone: true,
  templateUrl: './people-table.component.html',
  imports: [
    AsyncPipe
  ],
  styleUrls: ['./people-table.component.scss']
})
export class PeopleTableComponent implements OnInit, OnDestroy {
  private peopleService = inject(PeopleService);
  people$?: Observable<PersonViewModel[]>;

  selectedPersonId = model<number | 'new'>();
  personSelected = output<number | 'new' | undefined>();
  peopleUpdated = input<Observable<void>>();

  private onPeopleUpdatedSubscription?: Subscription;

  ngOnInit(): void {
    this.reloadPeople();
    this.onPeopleUpdatedSubscription = this.peopleUpdated()?.subscribe(() => this.reloadPeople());
  }

  ngOnDestroy(): void {
    this.onPeopleUpdatedSubscription?.unsubscribe();
  }

  reloadPeople(): void {
    this.people$ = this.peopleService.getAll();
  }

  select(person: PersonViewModel): void {
    this.personSelected.emit(person.id);
  }

  addPerson(): void {
    this.personSelected.emit('new');
  }

  deletePerson(person: PersonViewModel): void {
    // Who needs confirmation for permanently deleting data anyway?
    // (people with more time to work on a system)
    this.peopleService
      .delete(person.id)
      .subscribe(() => {
        // Deselect the editor if we delete the selected person
        if (this.selectedPersonId() == person.id) {
          this.personSelected.emit(undefined);
        }
        this.reloadPeople();
      });
  }
}
