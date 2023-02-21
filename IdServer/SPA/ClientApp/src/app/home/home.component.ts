import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { filter, from, map, Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ForecastService, WeatherForecast } from '../services/forecast.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isAuthenticated$: Observable<boolean>;
  user$: Observable<any>;
  accessTokenObj$: Observable<any>;
  forecasts$: Observable<WeatherForecast[]> = from([]);
  externalForecasts$: Observable<WeatherForecast[]> = from([]);
  user: string | null = null;
  constructor(
    private authService: AuthService,
    private forecastService: ForecastService
  ) {
    this.isAuthenticated$ = from(this.authService.isAuthenticated());
    this.user$ = this.authService.getUser();
    this.externalForecasts$ = this.forecastService.fetchExternal();
    this.accessTokenObj$ = this.authService.getTokenObj();
  }



  ngOnInit() {}

  fetch() {
    this.forecasts$ = this.forecastService.fetch();
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }
}
