import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-signincallback',
  template: ''
})
export class SigninCallbackComponent implements OnInit {

  constructor(private _router: Router, private _auth: AuthService){

  }
  ngOnInit(): void {
    var that = this;
    this._auth.callback(()=>{
      that._router.navigate(['/']);
    });
  }
}
