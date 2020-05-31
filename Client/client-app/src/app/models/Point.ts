import { AlarmType, PointType } from './Enums';
import { ConfigItem } from './ConfigItem';


/*
  Interface that represents Point
*/
export interface Point {
  address: number;
  alarm: AlarmType;
  commandedValue: 0;
  configItem: ConfigItem;
  displayValue: string;
  eguValue: number;
  error: string;
  name: string;
  pointId: number;
  rawValue: number;
  timestamp: string;
  type: PointType;
}
