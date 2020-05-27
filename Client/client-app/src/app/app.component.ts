import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from './services/signal-r.service';
import { environment } from 'src/environments/environment';
import { ResponseModel } from './models/ResponseModel';
import { PointType, AlarmType } from './models/Enums';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client-app';
  data: ResponseModel;
  displayedColumns: string[] = ['name', 'type', 'address', 'displayValue', 'rawValue', 'timestamp', 'alarm', 'command' ] ;
  constructor(public signalRService: SignalRService, private http: HttpClient) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();
    this.startHttpRequest();
    this.signalRService.navItem$.subscribe(data => this.showData(data));
  }

  showData(data: ResponseModel) {
    if (data !== null && data !== undefined) {
      this.data = data;
    }
  }

  status() {
    if(this.data === null || this.data === undefined){
      return false;
    } else if(this.data.status === 0){
      return true;
    } else {
      return false;
    }
  }

  private startHttpRequest = () => {
    this.http.get(`https://localhost:${environment.port}/api/app`)
      .subscribe(res => {
        console.log(res);
      });
  }
}
