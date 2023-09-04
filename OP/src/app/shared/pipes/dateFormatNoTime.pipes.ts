import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { environment } from 'src/environments/environment';

@Pipe({
  name: 'dateFormatNoTime'
})
export class DateFormatNoTimePipe implements PipeTransform {

  transform(value: any): any {
    if (value) {
      const date = moment(value).format(environment.formatDate);
      return date;
    }
    return null;
  }

}
