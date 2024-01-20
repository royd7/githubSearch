import { HttpClient, HttpErrorResponse, HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable, Injector } from "@angular/core";
import { Observable, catchError, switchMap, take, tap } from "rxjs";
import { environment } from "src/environments/environment";
import { UserService } from "./user.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private injector: Injector) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        console.log('Intercepted!', req);
        const authService = this.injector.get(UserService);
        const copiedReq = req.clone({
            headers: req.headers.set(
                'authorization', 'Bearer ' + authService.data
            )
        });

        if (!authService.data) {
            {

            }
        }

        return next.handle(copiedReq);
    }

}