import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, mergeMap, Observable, of } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class ForecastService {
  constructor(
    private authService: AuthService,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  fetch(): Observable<WeatherForecast[]> {
    var accessToken$ = this.authService.getAccessToken();
    return accessToken$.pipe(
      mergeMap((t) => this.getForecasts('weatherforecast', t))
    );
  }

  fetchExternal(): Observable<WeatherForecast[]> {
    var accessToken$ = this.authService.getAccessToken();
    return accessToken$
      .pipe(mergeMap((t) => this.getForecasts('weatherforecast/external', t)))
      .pipe(catchError((err) => of([])));
  }

  private getForecasts(
    path: string,
    token: string | null
  ): Observable<WeatherForecast[]> {
    var headers = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });

    var options = { headers };
    return this.http.get<WeatherForecast[]>(this.baseUrl + path, options);
  }
}

export interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
function to(
  arg0: (t: any) => Observable<WeatherForecast[]>
): import('rxjs').OperatorFunction<string | null, WeatherForecast[]> {
  throw new Error('Function not implemented.');
}
