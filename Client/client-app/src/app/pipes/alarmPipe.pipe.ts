import { Pipe, PipeTransform } from '@angular/core';
import { PointType, AlarmType } from '../models/Enums';

@Pipe({name: 'alarmPipe'})
export class AlarmPipe implements PipeTransform {
  transform(type: AlarmType): string {
    // tslint:disable-next-line: prefer-const
    let newStr = '';
    switch (type) {
      case AlarmType.ABNORMAL_VALUE: newStr = 'ABNORMAL_VALUE'; break;
      case AlarmType.HIGH_ALARM: newStr = 'HIGH_ALARM'; break;
      case AlarmType.LOW_ALARM: newStr = 'LOW_ALARM'; break;
      case AlarmType.NO_ALARM: newStr = 'NO_ALARM'; break;
      case AlarmType.REASONABILITY_FAILURE: newStr = 'REASONABILITY_FAILURE'; break;
    }
    return newStr;
  }
}
