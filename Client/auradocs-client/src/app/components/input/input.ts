import { Component, Input, input } from '@angular/core';

@Component({
  selector: 'app-input',
  imports: [],
  templateUrl: './input.html',
  styleUrl: './input.scss',
})
export class InputComponent {
  @Input() inputType:string = 'text';
  @Input() classList:string = 'primary-input';
  @Input() formControlName:string = '';

}
