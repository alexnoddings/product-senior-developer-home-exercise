import {Component, input} from '@angular/core';
import {KeyValuePipe} from "@angular/common";
import {ProblemDetails} from "@/app/models/problem-details";

@Component({
  selector: 'app-errors-outlet',
  standalone: true,
  imports: [ KeyValuePipe ],
  templateUrl: './errors-outlet.component.html'
})
export class ErrorsOutletComponent {
  error = input.required<ProblemDetails>();
}
