import { DatePipe } from '@angular/common';
import { environment } from 'src/environments/environment';

export class SearchDate {
    static searchFullDate(txtSearch): any {
        const regDate = /^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012]|[1-9])[- /.](19|20|21)\d\d$/;
        if (this.isValidTxtSearch(regDate, txtSearch) === true) {
            const st = txtSearch;
            const pattern = /(\d{1,2})[\/\-\.](\d{1,2})[\/\-\.](\d{4})/;
            const dt = new Date(st.replace(pattern, '$3-$2-$1'));
            //
            const datePipe = new DatePipe('en-US');
            let myOutDate = '';
            myOutDate = datePipe.transform(dt, environment.formatDateTime);
            return myOutDate;
        } else {
            return null;
        }
    }
    static isValidTxtSearch(regexp, txtSearch): any {
        return regexp.test(txtSearch);
    }
    static formatFilterDate(value: Date): any {
        if (value) {
            const date = new Date(value);
            return `${date.getDate()}-${date.getMonth() + 1}-${date.getFullYear()}`;
        }
        return value;
    }
    static formatDateTimeExcelName(value: Date): any {
        if (value) {
            const date = new Date(value);
            return `${date.getHours()}${date.getMinutes()}${date.getSeconds()}${date.getDate()}${date.getMonth() + 1}${date.getFullYear()}`;
        }
        return value;
    }
    static formatDateTimeExcel(value: Date): any {
        if (value) {
            const date = new Date(value);
            // 24/11/2020
            return `${ (date.getMonth() + 1) >= 10 ? date.getMonth() + 1 : '0' + (date.getMonth() + 1) }/${date.getDate() >= 10 ? date.getDate() : '0' + date.getDate() }/${date.getFullYear()}`;
        }
        return value;
    }
    static formatToISODate(date: any): any {
        const datePipe = new DatePipe('en-US');
        let myOutDate = '';
        myOutDate = datePipe.transform(date, environment.formatDateTime);
        return myOutDate;
    }

    static formatDateNoTime(date: any) {
        let datePipe = new DatePipe("en-US");
        let myOutDate: string = "";
        myOutDate = datePipe.transform(date, environment.formatDateTimeTable);
        return myOutDate;
    }
}
