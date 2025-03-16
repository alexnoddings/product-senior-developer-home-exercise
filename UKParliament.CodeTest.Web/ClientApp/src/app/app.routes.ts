import {Routes} from '@angular/router';
import {PeoplePageComponent} from "@/app/components/people/page/people-page.component";

export const routes: Routes = [
  {
    path: '',
    component: PeoplePageComponent,
    pathMatch: 'full',
    title: 'People'
  }
];
