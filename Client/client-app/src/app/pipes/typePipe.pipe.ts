import { Pipe, PipeTransform } from '@angular/core';
import { PointType } from '../models/Enums';


/*
  Pipe that converts integer value of PointType Enum from server to string
  Input:
        - value to convert
  Output:
         - converted value (string)
*/
@Pipe({name: 'typePipe'})
export class TypePipe implements PipeTransform {
  transform(type: PointType): string {
    // tslint:disable-next-line: prefer-const
    let newStr = '';
    switch (type) {
      case PointType.ANALOG_INPUT: newStr = 'ANALOG_INPUT'; break;
      case PointType.ANALOG_OUTPUT: newStr = 'ANALOG_OUTPUT'; break;
      case PointType.DIGITAL_INPUT: newStr = 'DIGITAL_INPUT'; break;
      case PointType.DIGITAL_OUTPUT: newStr = 'DIGITAL_OUTPUT'; break;
    }
    return newStr;
  }
}
