import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../models/ResponseModel';
import { BehaviorSubject } from 'rxjs';
import { HttpClient} from '@angular/common/http';

/*
  Service that provides SignalR Hub Connection
*/
@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private data: ResponseModel;
  public hubConnection: signalR.HubConnection;
  /// Data to observe
  private dataSource = new BehaviorSubject<ResponseModel>(this.data);
  /// Observable data
  dataSource$ = this.dataSource.asObservable();

  constructor(private http: HttpClient) { }

  /// Command to update data as observable data
  public change(model: ResponseModel) {
    this.dataSource.next(model);
  }

  /// Function to start connection to th server
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(`https://localhost:${environment.port}/hub`)
                            .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  /// Function witch adds data listener for messages from server
  public addDataListener = () => {
    this.hubConnection.on('recieveMsg', (data: ResponseModel) => {
      this.change(data);
    });
  }

  /// Function thaht initialize connection to the server
  public startHttpRequest = () => {
    this.http.get(`https://localhost:${environment.port}/api/app`)
      .subscribe(res => {
        console.log(res);
      });
  }

  /// Functions that sends get request, requesting log data from server
  public getLogs = () => {
    return this.http.get(`https://localhost:${environment.port}/api/app/logs`);
  }

  /// Function that sends command for executions on sever in post request
  public command = (pointId, address, value) => {
    return this.http.post(`https://localhost:${environment.port}/api/app/command`, {pointId, address, value});
  }
}
