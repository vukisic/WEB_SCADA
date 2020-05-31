
/*
  Class that represent Configuration item
*/
export class ConfigItem {
  abnormalValue: number;
  acquisitionInterval: number;
  decimalSeparatorPlace: number;
  defaultValue: number;
  description: string;
  // tslint:disable-next-line: variable-name
  egU_Max: number;
  // tslint:disable-next-line: variable-name
  egU_Min: number;
  highLimit: number;
  lowLimit: number;
  maxValue: number;
  minValue: number;
  numberOfRegisters: number;
  processingType: string;
  registryType: number;
  scaleFactor: number;
  secondsPassedSinceLastPoll: number;
  startAddress: number;
}
