import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../models/ResponseModel';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private data: ResponseModel;
  private hubConnection: signalR.HubConnection;
  private navItemSource = new BehaviorSubject<ResponseModel>(this.data);
  // Observable navItem stream
  navItem$ = this.navItemSource.asObservable();

  constructor() { }

  // service command
  changeNav(model: ResponseModel) {
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

  public addTransferChartDataListener = () => {
    this.hubConnection.on('recieveMsg', (data: ResponseModel) => {
      this.changeNav(data);
    });
  }
}
