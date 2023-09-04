import { Pipe, PipeTransform } from '@angular/core';
import * as moment from "moment";
import { environment } from 'src/environments/environment';

@Pipe({
  name: 'dateFormat'
})
export class DateFormatPipe implements PipeTransform {

  transform(value: any): any {
    if (value) {
      let date = moment(value).format(environment.formatDate)
      return date;
    }
    return null;
  }

}
