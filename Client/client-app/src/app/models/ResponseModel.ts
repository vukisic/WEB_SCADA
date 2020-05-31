import { Point } from './Point';

/*
  Class that represents response for update event from server
  status - status of connection to the simulator
  list - list of points and values
*/
export class ResponseModel {
  status: number;
  list: Point[];
}
