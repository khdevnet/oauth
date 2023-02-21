import { BrowserModule } from '@angular/platform-browser';
import { Injectable, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AuthModule, LogLevel } from 'angular-auth-oidc-client';
import { SigninCallbackComponent } from './signincallback/signincallback.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ForbiddenComponent,
    SigninCallbackComponent,
  ],
  imports: [
    AuthModule.forRoot({
      config: {
        authority: 'https://localhost:5001',
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        clientId: 'spa',
        scope: 'offline_access api1',
        responseType: 'id_token token',
        silentRenew: false,
        useRefreshToken: true,
        logLevel: LogLevel.Debug,
      },
    }),

    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'signin-callback', component: SigninCallbackComponent },
      { path: 'signout-callback', component: HomeComponent },
      { path: 'forbidden', component: ForbiddenComponent },
    ]),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
