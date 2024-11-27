import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { NavigationExtras, Router } from "@angular/router";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";


@Injectable()
export class ErrorInterceptor implements HttpInterceptor{

    constructor(private router:Router){

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(err =>{
                if(err){

                    if(err.status === 400){
                        if(err.error.errors){
                            console.log(err.error.errors)
                            throw err.error.errors
                        }
                        else{
                            console.log(err.error.message, err.error.statusCode)
                        }
                        
                    }
                    if(err.status === 401){
                        console.log(err.error.message, err.error.statusCode)
                    }

                    if(err.status === 404){
                        this.router.navigateByUrl('/not-found');
                    }
                    if(err.status === 500){
                        console.log(err.error)
                        const navigationExtras:NavigationExtras = {state: {error: err.error}}
                        this.router.navigateByUrl('/server-error',navigationExtras);
                    }
                    return throwError(err)
                }
            })
        )
    }
}