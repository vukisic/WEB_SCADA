import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../models/ResponseModel';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RequestModel } from '../models/RequestModel';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private data: ResponseModel;
  public hubConnection: signalR.HubConnection;
  private navItemSource = new BehaviorSubject<ResponseModel>(this.data);
  // Observable navItem stream
  navItem$ = this.navItemSource.asObservable();

  constructor(private http: HttpClient) { }

  // service command
  public changeNav(model: ResponseModel) {
    this.navItemSource.next(model);
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(`https://localhost:${environment.port}/hub`)
                            .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addDataListener = () => {
    this.hubConnection.on('recieveMsg', (data: ResponseModel) => {
      this.changeNav(data);
    });
  }

  public startHttpRequest = () => {
    this.http.get(`https://localhost:${environment.port}/api/app`)
      .subscribe(res => {
        console.log(res);
      });
  }

  public getLogs = () => {
    return this.http.get(`https://localhost:${environment.port}/api/app/logs`);
  }

  public command = (pointId, address, value) => {
    return this.http.post(`https://localhost:${environment.port}/api/app/command`, {pointId, address, value});
  }
}
