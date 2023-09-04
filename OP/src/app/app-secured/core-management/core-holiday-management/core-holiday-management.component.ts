import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Holiday } from 'src/app/shared/models/entity/Holiday.model';
import { HolidayService } from 'src/app/shared/services/api/holiday.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-core-holiday-management',
  templateUrl: './core-holiday-management.component.html',
  styleUrls: ['./core-holiday-management.component.scss']
})
export class CoreHolidayManagementComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  year: any = [{ label: "-- Tất cả --", data: null, value: 0 }];
  selectYear: number = 0;
  currentYear: any;
  yearNow: any;
  yearDaily: any = [];
  now: any
  selectDate: any;
  pageNumber: number;
  pageSize: number;
  holiday: Holiday[] = [];
  totalRecords: number;
  onPageChangeEvent: any;
  noteHoliday: string;
  dialogDelete = false;
  selectedData: any;
  dataHoliday: Holiday = new Holiday();
  formaTime = 'YYYY-MM-DD'

  constructor(
    protected holidayService: HolidayService,
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Quản lý ngày lễ' }
    ]);
    this.now = new Date();
    this.currentYear = this.now.getFullYear() - 1;
    //this.yearNow = this.now.getFullYear();
    this.yearNow = 0;
    this.selectDate = new Date();
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData() {
    this.pageNumber = 1;
    this.pageSize = 20;
    await this.loadHoliday();
    this.LoadYear();
  }

  async loadHoliday() {
    const results = await this.holidayService.getHolidayByYear(this.yearNow, this.pageNumber, this.pageSize);
    if (results.data.length > 0) {
      this.holiday = results.data;
      this.totalRecords = this.holiday[0].totalCount || 0;
    } else {
      this.holiday = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  LoadYear() {
    let arr = [];
    this.year = [];
    this.holiday.map( i => {
      if(!arr.includes(moment(i.date).year())){
        arr.push(moment(i.date).year())
      }
    })

    arr.sort((a,b) => b - a).map( i => {
      this.year.push({ label: i, data: null, value: i });
    })
  }

  changeYear() {
    this.pageNumber = 1;
    this.pageSize = 20;
    this.yearNow = this.selectYear;
    this.loadHoliday();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.pageSize = this.onPageChangeEvent.rows;
    this.loadHoliday();
  }

  refresher() {
    this.yearNow = 0;
    this.selectYear = null;
    this.noteHoliday = null;
    this.selectDate = new Date();
    this.pageNumber = 1;
    this.pageSize = 20;
    this.holiday = [];
    this.totalRecords = 0;
    this.first = 0;
    this.loadHoliday();
  }

  async deleteHoliday(): Promise<any> {
    const res = await this.holidayService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      await this.loadHoliday();
      this.LoadYear();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  async createHoliday() {
    this.dataHoliday.code = moment(this.selectDate).format(environment.formatDate);
    this.dataHoliday.notHoliday = this.noteHoliday;
    let dateTime = moment(this.selectDate).format(this.formaTime)
    this.dataHoliday.Date = new Date(dateTime);
    let res = await this.holidayService.create(this.dataHoliday);
    if (res.isSuccess) {
      this.msgService.success('Tạo thành ngày lễ công ');
      await this.loadHoliday();
      this.LoadYear();
    } else {
      let a = Object.values(res.data)
      if (a) {
        this.msgService.error('Tạo ngày lễ bị trùng');
      } else {
        this.msgService.error(a.toString() || 'Tạo không thành công');
      }
    }
  }
}
