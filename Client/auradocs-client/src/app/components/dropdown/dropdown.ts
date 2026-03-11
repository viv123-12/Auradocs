import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  Output,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropDownOptions } from '../../constants/app-constants';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dropdown.html',
  styleUrls: ['./dropdown.scss'],
})
export class Dropdown implements OnChanges {

  @Input() dropdownOptions: DropDownOptions<any>[] = [];
  @Input() selectedValue: number = 0;

  @Output() dropdownValueChanged = new EventEmitter<number>();

  public isListBoxOpen = false;
  public selectedOption = 0;

  constructor(private elementRef: ElementRef) {}

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['dropdownOptions'] && this.dropdownOptions.length) {

      const index = this.selectedValue
        ? this.dropdownOptions.findIndex( d => d.value == this.selectedValue)
        : -1;

      this.selectedOption = index >= 0 ? index : 0;
    }

    if (changes['selectedValue'] && this.selectedValue) {
      const index = this.dropdownOptions.findIndex( d => d.value == this.selectedValue);
      if (index >= 0) {
        this.selectedOption = index;
      }
    }
  }

  openDropDown() {
    this.isListBoxOpen = !this.isListBoxOpen;
  }

  selectOption(index: number) {
    this.selectedOption = index;
    this.selectedValue = this.dropdownOptions[index].value;
    this.isListBoxOpen = false;

    this.dropdownValueChanged.emit(index);
  }

  @HostListener('document:click', ['$event'])
  onClickedOutside(event: Event) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isListBoxOpen = false;
    }
  }
}
