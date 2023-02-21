import { Injectable } from '@angular/core';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { from, map, mergeMap, Observable, of, Subject } from 'rxjs';

export class Constants {
  public static apiRoot = 'https://localhost:5002/api';
  public static clientRoot = 'https://localhost:44418';
  public static idpAuthority = 'https://localhost:5001';
  public static clientId = 'spa';
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _userManager: UserManager;
  private _user: User | null;
  private _loginChangedSubject = new Subject<boolean>();
  public loginChanged = this._loginChangedSubject.asObservable();
  private get idpSettings(): UserManagerSettings {
    return {
      authority: Constants.idpAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}/signin-callback`,
      scope: 'openid profile email forecasts.read forecasts.loadexternal',
      response_type: 'code',
      post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`,
    };
  }

  constructor() {
    this._user = null;
    this._userManager = new UserManager(this.idpSettings);
  }

  public login = () => {
    return this._userManager.signinRedirect();
  };

  public logout = () => {
    return this._userManager.signoutRedirect();
  };

  public callback = (r: () => void) => {
    this._userManager
      .signinRedirectCallback()
      .then(function () {
        r();
      })
      .catch(function (e) {
        console.error(e);
      });
  };

  public getUser(): Observable<User | null> {
    return from(this._userManager.getUser());
  }

  public getTokenObj(): Observable<object> {
    return this.getAccessToken().pipe(map((t) => this.parseJwt(t)));
  }

  private parseJwt(token: string | null): Observable<object> {
    console.log(token);
    if (!token) return of({});
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(
      window
        .atob(base64)
        .split('')
        .map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );

    return JSON.parse(jsonPayload);
  }

  public getAccessToken(): Observable<string | null> {
    return this.getUser().pipe(map((u) => u!.access_token));
  }

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser().then((user) => {
      if (this._user !== user) {
        this._loginChangedSubject.next(this.checkUser(user));
      }
      this._user = user;

      return this.checkUser(user);
    });
  };

  private checkUser = (user: User | null): boolean => {
    return !!user && !user.expired;
  };
}
