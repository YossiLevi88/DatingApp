import { Injectable } from '@angular/core';
import { Resolve, Router, ResolveEnd, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { UserService } from '../services/user.service';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
    constructor(private userService: UserService, private authService: AuthService,
        // tslint:disable-next-line: align
        private alertify: AlertifyService, private router: Router) { }
 
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('Problame resolving your data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }

}
