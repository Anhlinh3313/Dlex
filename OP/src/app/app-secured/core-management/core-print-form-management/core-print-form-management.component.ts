import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { EntityBasic } from 'src/app/shared/models/entity/EntityBasic.model';
import { FromPrint } from 'src/app/shared/models/entity/fromPrint.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { GetListFormPrintViewModel } from 'src/app/shared/models/viewModel/getListFormPrint.viewModel';
import { FormPrintService } from 'src/app/shared/services/api/formPrintType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreatePrintFormComponent } from '../core-dialog-management/create-print-form/create-print-form.component';

@Component({
  selector: 'app-core-print-form-management',
  templateUrl: './core-print-form-management.component.html',
  styleUrls: ['./core-print-form-management.component.scss']
})
export class CorePrintFormManagementComponent extends BaseComponent implements OnInit {

  formPrints: SelectModel[] = [];
  formPrintTypes: SelectModel[] = [];
  formPrintsTab2: SelectModel[] = [];
  formPrintTypesTab2: SelectModel[] = [];
  selectedFormPrint: any = null;
  selectedFormPrintType: any = null;
  selectedFormPrintTab2: any = null;
  selectedFormPrintTypeTab2: any = null;
  typeId: number;
  selectedValues: any;
  Text: any;
  formPrintBody: any;
  ckeConfig: any;
  isNew: boolean;
  dataNumOrder: any;
  dataformPrints: FromPrint;
  cloneFromPrint: FromPrint;
  numOrder: any;
  isPublic: boolean;
  ref: DynamicDialogRef;
  width: number;
  height: number;
  listFormPrintViewModel: GetListFormPrintViewModel = new GetListFormPrintViewModel();
  listFormPrint: any;
  totalRecords: number;
  selectedData: any;
  dialogDelete = false;
  first = 0;
  index : number = 0;
  onPageChangeEvent: any;
  rows = 20;
  ck_editor : string;
  cke_content : string;
  @ViewChild("myckeditor") ckeditor: any;
  constructor(
    protected formPrintService: FormPrintService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Quản lý mẫu in' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }
  
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.ck_editor = document.getElementsByClassName("cke cke_reset").item(0).id;
      this.cke_content = document.getElementsByClassName("cke_contents cke_reset").item(0).id;
      
      document.getElementById(this.ck_editor).style.minWidth = '400px';
      document.getElementById(this.ck_editor).style.maxWidth = '100%';
      document.getElementById(this.cke_content).style.maxHeight = '500px';
    }, 1000);
  }

  intData() {
    this.loadFormPrint();
    this.getListFormPrint();
  }

  async loadFormPrint() {
    this.formPrints =  this.formPrintsTab2 = await this.formPrintService.getFormPrintTypeAsync();
  }

  async getListFormPrint() {
    if(this.selectedFormPrintTab2){
      if(this.selectedFormPrintTab2.value){
        this.listFormPrintViewModel.formPrintTypeId = this.selectedFormPrintTab2.value;
      }
    }
    
    if(this.selectedFormPrintTypeTab2){
      if(this.selectedFormPrintTypeTab2.value){
        this.listFormPrintViewModel.formPrintId = this.selectedFormPrintTypeTab2.value;
      }
    }
    let res =  await this.formPrintService.getListFormPrint(this.listFormPrintViewModel);
    if(res.isSuccess){
      this.listFormPrint = res.data;
      this.totalRecords = res.data[0]?.totalCount || 0;
    }
    else {
      this.listFormPrint = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  async onChangeFormPrint() {
    this.formPrintBody = "";
    document.getElementById(this.ck_editor).style.width ='100%';
    document.getElementById(this.cke_content).style.height ='100%';
    this.selectedFormPrintType = null;
    if (this.selectedFormPrint.value) {
      let FormPrintTypeId = this.selectedFormPrint.value;
      this.formPrintTypes = await this.formPrintService.getFormPrintAsync(FormPrintTypeId);
      if(this.isAdd){
        this.formPrintTypes.push({ value: 0, label: "-- Tạo mới mẫu in --" });
      }
    } else {
      this.formPrintTypes = [];
      this.width = null;
      this.height = null;
      this.numOrder = null;
      this.formPrintBody = null;
      this.isPublic = false;
    }
  }

  async onChangeFormPrintTab2() {
    this.selectedFormPrintTypeTab2 = null;
    this.listFormPrintViewModel.formPrintId = null;
    if (!this.selectedFormPrintTab2.value) {
      this.formPrintTypesTab2 = [];
      this.listFormPrintViewModel.formPrintTypeId = null;
    }
    else{
      let FormPrintTypeId = this.selectedFormPrintTab2.value;
      this.formPrintTypesTab2 = await this.formPrintService.getFormPrintAsync(FormPrintTypeId);
    }
    this.getListFormPrint();
  }

  onChangeFormPrintTypeTab2() {
    this.getListFormPrint();
  }



  onChange($event: any): void {
    //this.log += new Date() + "<br />";
  }

  openCreateModel(): void {
    this.ref = this.dialogService.open(CreatePrintFormComponent, {
      header: `${'TẠO MẪU IN MỚI'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: this.selectedFormPrint.value,
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadFormPrint();
        this.formPrintTypes = [];
        this.onChangeFormPrint()
      }
    });
  }

  async onChangeFormPrintType(event) {
    if (this.selectedFormPrintType.value != null) {
      if (this.selectedFormPrintType.value == 0) {
        this.openCreateModel();
        this.selectedFormPrintType = null;
      }
      else {
        let cloneSelectedFromPrint = this.selectedFormPrintType.value;
        this.formPrintTypes.forEach(x => {
          let obj = x.data as FromPrint;
          if (obj) {
            if (cloneSelectedFromPrint === obj.id) {
              this.dataformPrints = obj;
              this.changeFormPrint();
            }
            if (cloneSelectedFromPrint == null) {
              this.resetFrom();
            }
            if (obj.width && obj.height) {
              this.width = obj.width; 
              this.height = obj.height; 
              document.getElementById(this.ck_editor).style.width = `${obj.width}` + 'px';
              document.getElementById(this.cke_content).style.height = `${obj.height}` + 'px';
            }
          }
        });
      }
    } else {
      this.numOrder = null;
      this.width = null;
      this.height = null;
      this.formPrintBody = null;
      this.isPublic = false;
    }
  }

  changeFormPrint() {
    let fromPrint = this.dataformPrints;
    this.cloneFromPrint = Object.assign({}, fromPrint);
    this.resetFrom()
    if (fromPrint) {
      this.numOrder = fromPrint.numOrder;
      this.formPrintBody = fromPrint.formPrintBody;
      this.isPublic = fromPrint.isPublic;
    }
  }

  resetFrom() {
    this.width = null;
    this.height = null;
    this.numOrder = null;
    this.formPrintBody = null;
    this.isPublic = false;
  }

  async updateFormPrint() {
    this.messageService.clear();
    var data = new FromPrint();
    let cloneSelectedFromPrint = this.selectedFormPrintType.value;
    this.formPrintTypes.forEach(x => {
      let obj = x.data as FromPrint;
      if (obj) {
        if (cloneSelectedFromPrint === obj.id) {
          data = obj;
        }
      }
    });
    
    data.formPrintBody = this.formPrintBody;
    data.numOrder = this.numOrder;
    data.isPublic = this.isPublic;
    data.height = this.height;
    data.width = this.width;
    await this.formPrintService.update(data).then(
      x => {
        if (!this.isValidResponse(x)) return;
        let result = x.data;
        data.concurrencyStamp = result.concurrencyStamp;
        this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
        if (result.width) {
          document.getElementById(this.ck_editor).style.width = `${result.width}` + 'px';
        }
        if (result.height) {
          document.getElementById(this.cke_content).style.height = `${result.height}` + 'px';
        }
      }
    );
  }

  editFormPrint(rowData : any){
    this.index = 1;
    this.formPrints.map( async i =>  {
      if(i.value == rowData.formPrintTypeId){
        this.selectedFormPrint = i;
        await this.onChangeFormPrint();
      }
    })

    setTimeout(() => {
      this.formPrintTypes.map(i => {
        if(i.value == rowData.id){
          this.selectedFormPrintType = i;
          this.onChangeFormPrintType(null);
        }
      })
    }, 300);
    
    this.numOrder = rowData.numOrder;
    this.isPublic = rowData.isPublic;
  }

  handleChange(event){
    this.index = event.index;
  }

  async deleteFormPrint(): Promise<any> {
    const res = await this.formPrintService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.getListFormPrint();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.listFormPrintViewModel.pageNum = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.listFormPrintViewModel.pageSize = this.onPageChangeEvent.rows;
    this.getListFormPrint();
  }

  onSearch() {
    this.first = 0;
    this.listFormPrintViewModel.pageNum = 1;
    this.listFormPrintViewModel.searchText = this.listFormPrintViewModel.searchText.trim();
    this.getListFormPrint();
  }
}
