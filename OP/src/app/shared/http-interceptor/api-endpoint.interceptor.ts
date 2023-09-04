import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Constant } from '../infrastructure/constant';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { StorageService } from '../services/local/storage.service';

import 'rxjs/add/operator/do';
import { MsgService } from '../services/local/msg.service';

@Injectable()
export class ApiEndpointInterceptor implements HttpInterceptor {
    constructor(
        private router: Router,
        private msgService: MsgService,
        private storageService: StorageService
    ) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.storageService.get(Constant.auths.token);
        const tokenFirebase = this.storageService.get(Constant.auths.tokenFirebase);
        let modReq = null;

        if (req.url === 'https://fcm.googleapis.com/fcm/send') {
            const authHeader = 'key=' + tokenFirebase;
            const updUrl = { url: req.url, headers: req.headers.set('Authorization', authHeader) };
            modReq = req.clone(updUrl);
        }
        else if (token) {
            const authHeader = 'Bearer ' + token;
            const updUrl = { url: req.url, headers: req.headers.set('Authorization', authHeader) };
            modReq = req.clone(updUrl);
        } else {
            const url = { url: req.url };
            modReq = req.clone(url);
        }

        return next.handle(modReq).do((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {
                // do stuff with response if you want
            }
        }, (err: any) => {
            if (err instanceof HttpErrorResponse) {
                if (err.status === 401) {
                    // redirect to the login route
                    // or show a modal
                    this.storageService.removeAll();
                    this.router.navigate(['/login']);
                    this.msgService.warn('Hết phiên làm việc');
                }
                if (err.status === 403) {
                    // redirect to the login route
                    // or show a modal
                    // console.log("403");
                    this.router.navigate(['/403']);
                }
            }
        });
    }
}
