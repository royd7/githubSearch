import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { UserService } from './user.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private authService: UserService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot, state: RouterStateSnapshot,
  ) {
    const isAuth = this.authService.isAuthorized();
    if (!isAuth) {
      this.authService.login();
      return true;
    }
    return this.authService.isAuthorized();
  }
}