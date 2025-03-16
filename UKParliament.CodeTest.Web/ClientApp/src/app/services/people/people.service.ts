import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CreatePersonModel} from "@/app/models/people/create-person-model";
import {UpdatePersonModel} from "@/app/models/people/update-person-model";
import {PersonViewModel} from "@/app/models/people/person-view-model";

@Injectable({
  providedIn: 'root'
})
export class PeopleService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {
  }

  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(this.baseUrl + `api/people/${id}`)
  }

  getAll(): Observable<PersonViewModel[]> {
    // abc
    return this.http.get<PersonViewModel[]>(this.baseUrl + `api/people`)
  }

  create(person: CreatePersonModel): Observable<number> {
    return this.http.post<number>(this.baseUrl + `api/people`, person)
  }

  update(id: number, person: UpdatePersonModel): Observable<void> {
    return this.http.put<void>(this.baseUrl + `api/people/${id}`, person);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + `api/people/${id}`)
  }
}
