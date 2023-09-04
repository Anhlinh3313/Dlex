import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PriceService } from 'src/app/shared/models/entity/priceService.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PriceServiceService } from 'src/app/shared/services/api/priceService.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-copy-price',
  templateUrl: './core-copy-price.component.html',
  styleUrls: ['./core-copy-price.component.scss']
})
export class CoreCopyPriceComponent extends BaseComponent implements OnInit {

  newPriceServiceCode: string;
  priceService: PriceService = new PriceService();

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    private priceServiceService: PriceServiceService,
  ) {
    super(msgService, router, permissionService);

  }

  ngOnInit(): void {
    this.intData();
  }

  async intData() {
    if (this.config.data) {
      this.priceService = this.config.data;
    }
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  copyPriceService() {
    if (!this.priceService || !this.priceService.id || this.priceService.id == 0) {
      this.msgService.error("Vui lòng chọn bảng giá cần copy!!!");
      return;
    }
    if (!this.newPriceServiceCode) {
      this.msgService.error("vui lòng nhập mã bảng giá để copy.");
      return;
    }

    this.priceServiceService.CopyPriceServiceAsync(this.priceService.id, this.newPriceServiceCode).then(
      x => {
        if (!this.isValidResponse(x)) return;
        this.msgService.error("Copy bảng giá dịch vụ thành công.");
        this.newPriceServiceCode = null;
        this.priceService = new PriceService();
        this.onClickCancel(true);
      }
    )
  }
}
