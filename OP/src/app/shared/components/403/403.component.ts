import { Component, OnInit } from '@angular/core';
import { AppSecuredComponent } from 'src/app/app-secured/app-secured.component';
import { environment } from 'src/environments/environment';
import { AuthService } from '../../services/api/auth.service';
import { MsgService } from '../../services/local/msg.service';

@Component({
    selector: 'app-403',
    templateUrl: './403.component.html',
    styles: [
    ]
})
export class Page404Component implements OnInit {

    constructor(private authService: AuthService, private msgService: MsgService) { }
    ngOnInit() {
    }
    
    logout(): void {
        this.authService.logout();
    }
}
