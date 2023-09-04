import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppSecuredComponent } from 'src/app/app-secured/app-secured.component';
import { environment } from 'src/environments/environment';
import { Page } from '../../models/entity/page.model';
import { PageService } from '../../services/api/page.service';
import { PermissionService } from '../../services/api/permission.service';
import { MsgService } from '../../services/local/msg.service';
import { BaseComponent } from '../baseComponent';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styles: [
    ]
})
export class MenuComponent extends BaseComponent implements OnInit {

    model: any[] = [];
    envir = environment;
    pages: Page[];


    constructor(
        public app: AppSecuredComponent,
        protected pageService: PageService,
        protected msgService: MsgService,
        protected router: Router,
        protected permissionService: PermissionService,  
    ) {
        super(msgService,router, permissionService);
     }

    async ngOnInit(): Promise<any> {
        // this.pages = (await this.pageService.getMenuByModuleIdAsync(1)).data;
        // this.pages.forEach(page => {
        //     if (!page.parentPageId) {
        //         const menu = { label: page.name, icon: page.icon, routerLink: [page.aliasPath],
        //             img: 'assets/layout/images/topbar/dashboard.png',
        //             imgActive: 'assets/layout/images/topbar/dashboard_active.png',items: [] };
        //         menu.items = this.pages.filter(x => x.parentPageId === page.id).map(child => {
        //             return { 
        //                 label: child.name, icon: child.icon, routerLink: [child.aliasPath],
        //                 img: 'assets/layout/images/topbar/dashboard.png',
        //                 imgActive: 'assets/layout/images/topbar/dashboard_active.png',
        //             };
        //         });
        //         this.model.push(menu);
        //     }
        // });
        // const UserId = this.authService.getUserId();
        // const res = await this.pageService.getMenuByUser();
        // if (res.isSuccess){
        //     const data = res.data as PageModel[];
        //     data.map(x => {
        //         if (x.pageId == null){
        //             x.children = data.filter(y => y.pageId === x.id);
        //             this.model.push(x);
        //         }

        //     });
        // }
        this.model = [
            {
                label: 'Dashboard',
                img: 'assets/layout/images/topbar/dashboard.png',
                imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                routerLink: ['/']
            },    
            {
                label: 'Quản lý nhân viên',
                img: 'assets/layout/images/topbar/dashboard.png',
                imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                routerLink: ['/core-general'],
                items: [
                    {
                        label: 'Tài khoản',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-general/users']
                    },
                    {
                        label: 'Chức vụ',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-general/roles']
                    },
                    {
                        label: 'Phân quyền',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-general/role-pages']
                    },
                    {
                        label: 'Nhóm nhân viên',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-general/core-user-relation']
                    },
                ]
            },
            {
                label: 'Quản lý địa danh',
                img: 'assets/layout/images/topbar/dashboard.png',
                imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                routerLink: ['/core-place'],
                items: [
                    {
                        label: 'Quản lý quốc gia',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-place/country']
                    },
                    {
                        label: 'Quản lý tỉnh thành',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-place/province']
                    },
                    {
                        label: 'Quản lý quận huyện',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-place/district']
                    },
                    {
                        label: 'Quản lý phường xã',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-place/ward']
                    },
                ]
            },
            {
                label: 'Quản lý hệ thống',
                img: 'assets/layout/images/topbar/dashboard.png',
                imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                routerLink: ['/core-hub'],
                items: [
                    {
                        label: 'Quản lý trung tâm',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-hub/hub']
                    },
                    {
                        label: 'Quản lý chi nhánh',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-hub/po-hub']
                    },
                    {
                        label: 'Quản lý kho trạm',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-hub/station-hub']
                    },
                    {
                        label: 'Phân khu vực phục vụ',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-hub/route-hub']
                    },
                    {
                        label: 'Phân tuyến giao nhận',
                        img: 'assets/layout/images/topbar/dashboard.png',
                        imgActive: 'assets/layout/images/topbar/dashboard_active.png',
                        routerLink: ['/core-hub/routing-hub']
                    },
                ]
            },
        ];
    }

    onMenuClick(): void {
        this.app.menuClick = true;
    }
}
