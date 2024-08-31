import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { User } from '../models/models';
import { NavigationService } from '../services/navigation.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  invaildRPWD: boolean = false;
  message = '';

  constructor(
    private fb: FormBuilder,
    private navigationService: NavigationService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      firstName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.pattern('[a-zA-Z].*'),
        ],
      ],
      lastName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.pattern('[a-zA-Z].*'),
        ],
      ],
      email: ['', [Validators.required, Validators.email]],
      address: ['', [Validators.required]],
      mobile: ['', Validators.required],
      pwd: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(15),
        ],
      ],
      rpwd: [''],
    });
  }

  register() {
    let user: User = {
      id: 0,
      firstName: this.FirstName.value,
      lastName: this.LastName.value,
      email: this.Email.value,
      address: this.Address.value,
      mobile: this.Mobile.value,
      password: this.PWD.value,
      createdAt: '',
      modifiedAt: '',
    };

    this.navigationService.registerUser(user).subscribe((res: any) => {
      this.message = res.toString();
    });

    this.navigationService
    .registerUser(user)
    .subscribe({
      next: (res: any) => {
        // if (res.toString() !== 'invalid') {
          this.message = res;
         
        // } else {
          // this.message = 'Invalid Credentials!';
        // }
      },
      error: (err) => {
        // Error handling logic
        console.log('erooor');
        console.log(err);
        if (err.error) {
          // Backend may return a structured error with a message property
          this.message = err.error;
        } else if (err.message) {
          // If err object has a message property directly
          this.message = err.message;
        } else {
          // Default fallback message if no specific error message is found
          this.message = 'An error occurred while trying to log in. Please try again later.';
        }
      }
    });
  }

  //#region Getters
  get FirstName(): FormControl {
    return this.registerForm.get('firstName') as FormControl;
  }
  get LastName(): FormControl {
    return this.registerForm.get('lastName') as FormControl;
  }
  get Email(): FormControl {
    return this.registerForm.get('email') as FormControl;
  }
  get Address(): FormControl {
    return this.registerForm.get('address') as FormControl;
  }
  get Mobile(): FormControl {
    return this.registerForm.get('mobile') as FormControl;
  }
  get PWD(): FormControl {
    return this.registerForm.get('pwd') as FormControl;
  }
  get RPWD(): FormControl {
    return this.registerForm.get('rpwd') as FormControl;
  }
  //#endregion
}
