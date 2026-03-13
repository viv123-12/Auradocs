import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { DropDownOptions } from '../../constants/app-constants';
import { Dropdown } from '../../components/dropdown/dropdown';
import { Switch } from '../../components/switch/switch';
import { UserType } from '../../shared/enums/user-type.enum';
import { Authentication, RegisterRequest } from '../../services/authentication';
import { MasterDataService } from '../../services/master-data-service';

type DomainPracticeAreas = {
  [key: string]: DropDownOptions<number>[];
};


@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, Dropdown,Switch,CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register implements OnInit{
  @ViewChild(Dropdown) child!: Dropdown;
  registerForm = new FormGroup({
    email:new FormControl('',[Validators.maxLength(125),Validators.email,Validators.required]),
    phoneNumber: new FormControl('',[Validators.maxLength(10),Validators.required,Validators.pattern(/^\d{10}$/)]),
    organizationId: new FormControl('',[Validators.maxLength(20),Validators.pattern(/^\d{10}$/)])
  });
  public domainDropDown:DropDownOptions<number>[] = [];
  public modesOfRegistration:string[] = [UserType.INDIVIDUAL,UserType.ORGANIZATION];
  public isOrganization:boolean = false;
  private domainPracticeAreas: DomainPracticeAreas = {};
  public registerRequest:RegisterRequest;
  public practiceArea:DropDownOptions<number>[] = [];
  public selectedDomain:number = 0;
  public selectedPracticeArea:number = 0;
  public currentMode:string = UserType.INDIVIDUAL;

  constructor(private authentication:Authentication,
    private router:Router,
    private masterData:MasterDataService,
    private cdr: ChangeDetectorRef){
    this.registerRequest = {
      userId: '',
      phoneNumber:  '',
      accountType: '',
      domainType: 0,
      practiceArea:  0
    };
  }
  

  ngOnInit(): void {
    this.masterData.getDomainList().subscribe(
      {
        next:
          res => {
            if (res.status == 200)
            {
              this.domainDropDown = JSON.parse(res.body);
              this.cdr.markForCheck();
              this.selectedDomain = this.domainDropDown[0].value;

              this.masterData.getPracticeAreaList().subscribe(
              {
                next:
                  res => {
                    if(res.status == 200)
                    {
                      this.domainPracticeAreas = JSON.parse(res.body);
                      this.cdr.markForCheck();
                      this.practiceArea = this.domainPracticeAreas[this.selectedDomain];
                      this.selectedPracticeArea = this.domainPracticeAreas[this.selectedDomain][0].value;
                    }
                  },
                error:
                  error => {
                    console.error(error);
                    return;
                  }
              }
            )}
          },
        
        error: 
          error => {
            console.error(error);
            return;
          }
      }
    );
  }
  public onFormSubmission(): void {
    if (!this.registerForm.valid) {
      console.error('Form is invalid');
      return;
    }

    const formValue = this.registerForm.value;
    this.registerRequest = {
      userId: (this.isOrganization ? formValue.organizationId : formValue.email) ?? '',
      phoneNumber: formValue.phoneNumber ?? '',
      accountType: this.currentMode,
      domainType: this.selectedDomain ?? 0,
      practiceArea: this.selectedPracticeArea ?? 0
    };
    
    this.registerForm.reset();
    this.authentication.registerUser(this.registerRequest , this.currentMode).subscribe({
      next:
        res => {
          if(res.status == 200)
          {
            this.router.navigate(['/'])
          }
        },
      error:
        error => {
          console.error(error);
          this.registerForm.reset();
          return;
        }
    })
  }

  public onDomainDropDownChange(value:number){
    this.selectedDomain = this.domainDropDown[value].value;
    this.practiceArea = this.domainPracticeAreas[this.domainDropDown[value].label]; 
  }

  public onDomainPracticeDropDownChange(value:number){
    this.selectedPracticeArea = this.practiceArea[value].value;
  }

  public onToogleValuechange(value:string){
    this.currentMode = value;
    if(value == UserType.INDIVIDUAL)
    { 
      this.isOrganization = false;
    }else{
      this.isOrganization = true;
      const organizationIdControl = this.registerForm.get('organizationId');
      organizationIdControl?.setValidators([
        Validators.required,
        Validators.pattern(/^\d{10}$/)
      ]);
    } 
 }
 get f()
 {
  return this.registerForm.controls;
 }
}
