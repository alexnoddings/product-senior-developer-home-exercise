import {Component} from '@angular/core';

@Component({
  selector: 'app-person-editor-empty',
  standalone: true,
  templateUrl: './person-editor-empty.component.html',
  // Pinch the styling from the main editor
  styleUrls: ['../form/person-editor-form.component.scss']
})
export class PersonEditorEmptyComponent {
}
