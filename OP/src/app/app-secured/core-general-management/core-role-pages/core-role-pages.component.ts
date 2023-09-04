import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Page } from 'src/app/shared/models/entity/page.model';
import { RolePage } from 'src/app/shared/models/entity/rolePage.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { ModulePageService } from 'src/app/shared/services/api/modulePage.service';
import { PageService } from 'src/app/shared/services/api/page.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-role-pages',
  templateUrl: './core-role-pages.component.html',
  styleUrls: ['./core-role-pages.component.scss']
})
export class CoreRolePagesComponent extends BaseComponent implements OnInit {
  // Data
  modules: SelectModel[] = [];
  roles: SelectModel[] = [];
  pages: Page[] = [];
  //
  selectedModule: SelectModel;
  selectedRole: SelectModel;
  dialogDelete = false;
  itemAllRolePage: boolean;
  rolePageItem: boolean = false;

  constructor(
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected modulePage: ModulePageService,
    protected roleService: RoleService,
    protected pageService: PageService,
    protected permissionService: PermissionService,
    protected router: Router
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Phân quyền' }
    ]);
  }

  ngOnInit(): void {
    this.initData();
  }

  async initData(): Promise<any> {
    await this.getModulePages();
    await this.getRoles();
  }

  async getModulePages(): Promise<any> {
    this.modules = await this.modulePage.getAllSelectModelAsync();
  }

  async getRoles(): Promise<any> {
    this.roles = await this.roleService.getAllSelectModelAsync();
  }

  async getPageByModuleId(): Promise<any> {
    this.pages = [];
    this.selectedRole = null;
    const result = await this.pageService.getPageByModuleId(this.selectedModule.value);
    if (result.data) {
      const pageModules = result.data as Page[];
      pageModules.forEach(element => {
        if (!element.parentPageId) {
          element.children = pageModules.filter(x => x.parentPageId === element.id || element.id === x.id);
          //
          const rolePage = new RolePage();
          rolePage.id = 0;
          rolePage.pageId = element.id;
          rolePage.roleId = null;
          rolePage.isAccess = false;
          rolePage.isAdd = false;
          rolePage.isDelete = false;
          rolePage.isEdit = false;
          element.rolePage = rolePage;

          element.children.forEach(child => {
            const rolePageChildren = new RolePage();
            rolePageChildren.id = 0;
            rolePageChildren.pageId = child.id;
            rolePageChildren.roleId = null;
            rolePageChildren.isAccess = false;
            rolePageChildren.isAdd = false;
            rolePageChildren.isDelete = false;
            rolePageChildren.isEdit = false;
            child.rolePage = rolePageChildren;
          });
          //
          this.pages.push(element);
        }
      });
    }
  }

  async getPermissionByRoleId(): Promise<any> {
    //const result = await this.permissionService.getPermissionByRoleId(this.selectedRole.value);
    const result = await this.permissionService.getAllPermissionByRoleId(this.selectedRole.value);
    if (result.data) {
      const rolePages = result.data as RolePage[];
      this.pages.forEach(element => {
        const rolePage = rolePages.filter(x => x.pageId === element.id && x.roleId === this.selectedRole.value);
        if (rolePage.length > 0) {
          element.rolePage = rolePage[0];
        } else {
          const rolePageEmpty = new RolePage();
          rolePageEmpty.id = 0;
          rolePageEmpty.pageId = element.id;
          rolePageEmpty.roleId = this.selectedRole.value;
          rolePageEmpty.isAccess = false;
          rolePageEmpty.isAdd = false;
          rolePageEmpty.isDelete = false;
          rolePageEmpty.isEdit = false;
          element.rolePage = rolePageEmpty;
        }
        element.children.forEach(child => {
          const rolePageChildren = rolePages.filter(x => x.pageId === child.id && x.roleId === this.selectedRole.value);
          if (rolePageChildren.length > 0) {
            child.rolePage = rolePageChildren[0];
          } else {
            const rolePageEmpty = new RolePage();
            rolePageEmpty.id = 0;
            rolePageEmpty.pageId = element.id;
            rolePageEmpty.roleId = this.selectedRole.value;
            rolePageEmpty.isAccess = false;
            rolePageEmpty.isAdd = false;
            rolePageEmpty.isDelete = false;
            rolePageEmpty.isEdit = false;
            child.rolePage = rolePageEmpty;
          }
        });
        //
      });
    }
  }

  async updatePermission(): Promise<any> {
    const rolePages: RolePage[] = [];

    this.pages.forEach(element => {
      rolePages.push(element.rolePage);

      element.children.forEach(child => {
        child.rolePage.roleId = this.selectedRole.value;
        child.rolePage.pageId = child.id;
        child.rolePage.pageId = child.id;
        child.rolePage.isEnabled = true;
        const findExist = rolePages.find(f => f.roleId === this.selectedRole.value && f.pageId === child.id);
        if (!findExist) {
          rolePages.push(child.rolePage);
        }
      });
    });
    this.dialogDelete = false;
    const result = await this.permissionService.updatePermission(rolePages);
    const permissions = result.data as RolePage[];
    this.pages.forEach(element => {
      element.rolePage = rolePages.filter(x => x.pageId === element.id && x.roleId === this.selectedRole.value)[0];

      element.children.forEach(child => {
        child.rolePage = rolePages.filter(x => x.pageId === child.id && x.roleId === this.selectedRole.value)[0];
      });
    });
    this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
  }

  // CheckAllRolePage(item, ev) {
  //   for (let i = 0; i < item.length; i++) {
  //     item[i].rolePage.checkRolePageId = ev.checked;
  //     this.changeRole(item[i]);
  //   }
  // }

  changeRole(item) { 
    item.rolePage.isAccess = item.rolePage.checkRolePageId;
    item.rolePage.isAdd = item.rolePage.checkRolePageId;
    item.rolePage.isEdit = item.rolePage.checkRolePageId;
    item.rolePage.isDelete = item.rolePage.checkRolePageId;
  }

  changeisAccess(item,event){
    if(item.rolePage.isAccess && item.rolePage.isAdd && item.rolePage.isEdit && item.rolePage.isDelete){
      item.rolePage.checkRolePageId = true;
      item.rolePage.isAccess = event.checked;
    }else{
      item.rolePage.checkRolePageId = false;
      item.rolePage.isAccess = event.checked;
    }
  }

  changeisAdd(item,event){
    if(item.rolePage.isAccess && item.rolePage.isAdd && item.rolePage.isEdit && item.rolePage.isDelete){
      item.rolePage.checkRolePageId = true;
      item.rolePage.isAdd = event.checked;
    }else{
      item.rolePage.checkRolePageId = false;
      item.rolePage.isAdd = event.checked;
    }
  }

  changeisEdit(item,event){
    if(item.rolePage.isAccess && item.rolePage.isAdd && item.rolePage.isEdit && item.rolePage.isDelete){
      item.rolePage.checkRolePageId = true;
      item.rolePage.isEdit = event.checked;
    }else{
      item.rolePage.checkRolePageId = false;
      item.rolePage.isEdit = event.checked;
    }
  }

  changeisDelete(item,event){
    if(item.rolePage.isAccess && item.rolePage.isAdd && item.rolePage.isEdit && item.rolePage.isDelete){
      item.rolePage.checkRolePageId = true;
      item.rolePage.isDelete = event.checked;
    }else{
      item.rolePage.checkRolePageId = false;
      item.rolePage.isDelete = event.checked;
    }
  }
}
