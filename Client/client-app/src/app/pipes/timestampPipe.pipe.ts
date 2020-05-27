import { Pipe, PipeTransform } from '@angular/core';
import { PointType } from '../models/Enums';

@Pipe({name: 'timestampPipe'})
export class TimestampPipe implements PipeTransform {
  transform(value: string): string {
    // tslint:disable-next-line: prefer-const
    let newStr = `${this.invertDate(value.split('T')[0])} ${value.split('T')[1].split('.')[0]}`;
    return newStr;
  }

  invertDate(date) {
    return `${date.split('-')[2]}-${date.split('-')[1]}-${date.split('-')[0]}`;
  }
}
