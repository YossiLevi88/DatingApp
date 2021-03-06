import { Injectable } from '@angular/core';
import { Resolve, Router, ResolveEnd, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { UserService } from '../services/user.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<User[]> {
    pageNumber = 1;
    pageSize = 5;
    likesParams = 'Likers';

    constructor(private userService: UserService,
        // tslint:disable-next-line: align
        private alertify: AlertifyService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParams).pipe(
            catchError(error => {
                this.alertify.error('Problame resolving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
