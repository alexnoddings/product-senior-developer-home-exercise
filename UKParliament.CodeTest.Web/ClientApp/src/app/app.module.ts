import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {routes} from "@/app/app.routes";

import {AppComponent} from '@/app/app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    RouterModule.forRoot(routes),
    ReactiveFormsModule,
  ],
  providers: [provideHttpClient(withInterceptorsFromDi())]
})
export class AppModule {
}
