import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { CustomerPriceServiceModel } from 'src/app/shared/models/entity/customerPriceService.model';
import { PriceService } from 'src/app/shared/models/entity/priceService.model';
import { CustomerPriceServiceService } from 'src/app/shared/services/api/customer-price-service.service';
import { CustomerService } from 'src/app/shared/services/api/customer.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PriceServiceService } from 'src/app/shared/services/api/priceService.service';
import { PromotionService } from 'src/app/shared/services/api/promotion.service';
import { PromotionCustomerService } from 'src/app/shared/services/api/promotionCustomer.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-applying-price-customer',
  templateUrl: './core-applying-price-customer.component.html',
  styleUrls: ['./core-applying-price-customer.component.scss']
})
export class CoreApplyingPriceCustomerComponent extends BaseComponent implements OnInit {
  rows = 20;
  first = 0;
  totalRecords: number;

  resultPriceServices: string[] = [];
  selectedCustomer: CustomerPriceServiceModel[] = [];
  listPriceService: PriceService[] = [];

  priceService: PriceService = new PriceService();

  selectedPriceService: number;

  colsCustomerPriceService = [
    { field: 'code', header: 'Mã' },
    { field: 'customer.name', header: 'Khách hàng' },
    { field: 'vatPercent', header: '%VAT' },
    { field: 'fuelPercent', header: '%PPXD' },
    { field: 'remoteAreasPricePercent', header: '%VSVX' },
    { field: 'dim', header: 'DIM' },
  ];

  constructor(
    protected customerService: CustomerService,
    protected promotionCustomerService: PromotionCustomerService,
    protected promotionService: PromotionService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    private priceServiceService: PriceServiceService,
    private customerPriceService: CustomerPriceServiceService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý bảng giá' },
      { label: 'Áp giảm giá khách hàng' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData(): void {
  }

  searchByCode(event, dropdownClick?): void {
    let value = event.query;
    if (dropdownClick) {
      value = event.query ? event.query : '%';
    }
    if (value.length >= 1) {
      this.priceServiceService.getByCodeAsync(value).then(
        x => {
          this.listPriceService = x as PriceService[];
          this.resultPriceServices = this.listPriceService.map(f => f.code);
        }
      );
    }
  }
  onSelectedByCode(event): void {
    const value = event;
    this.findByCode(value);
  }
  onClickByCode(): void {
    const value = this.priceService.code;
    this.findByCode(value);
  }
  keyUpEnter(event): void {
    if (event.code === 'Enter') {
      const value = event.currentTarget.value;
      this.findFilterByCode(value);
    }
  }

  findByCode(code: string): void {
    // this.messageService.clear();
    if (code.length >= 1) {
      const findPS = this.listPriceService.find(f => f.code.toLocaleUpperCase() === code.toLocaleUpperCase());
      if (findPS) {
        this.priceService = findPS;
        this.selectedPriceService = findPS.id;
        this.getCustomerPriceService();
      } else {
        this.priceService = new PriceService();
        this.selectedPriceService = null;
        this.selectedCustomer = [];
      }
    }
  }

  async findFilterByCode(code: string): Promise<void> {
    if (code.length >= 1) {
      const findPS = this.listPriceService.find(f => f.code.toLocaleUpperCase().indexOf(code.toLocaleUpperCase()) > -1);
      if (findPS) {
        this.priceService = findPS;
        this.selectedPriceService = findPS.id;
        this.getCustomerPriceService();
      } else {
        const results = await this.priceServiceService.getByCodeAsync(code);
        if (results.length > 0) {
          this.listPriceService = results as PriceService[];
          const findPSR = this.listPriceService.find(f => f.code.toLocaleUpperCase().indexOf(code.toLocaleUpperCase()) > -1);
          if (findPSR) {
            this.priceService = findPSR;
            this.selectedPriceService = findPSR.id;
            this.getCustomerPriceService();
          } else {
            this.priceService = new PriceService();
            this.priceService.code = code;
            this.selectedPriceService = null;
            this.selectedCustomer = [];
          }
        }
      }
    }
  }

  async getCustomerPriceService(): Promise<any> {
    if (this.selectedPriceService) {
      const result = await this.customerPriceService.GetByPriceServiceId(this.selectedPriceService);
      this.selectedCustomer = result;
      this.totalRecords = this.selectedCustomer.length;
    }
    else {
      this.selectedCustomer = [];
    }
  }
}
