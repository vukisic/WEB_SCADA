import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ResponseModel } from 'src/app/models/ResponseModel';
import { SignalRService } from 'src/app/services/signal-r.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  data: ResponseModel;
  log: string[] = [];
  constructor(public signalRService: SignalRService, private http: HttpClient) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addDataListener();
    this.signalRService.startHttpRequest();
    this.signalRService.navItem$.subscribe(data => this.showData(data));
  }

  showData(data: ResponseModel) {
    if (data !== null && data !== undefined) {
      this.data = data;
    }
  }

  status() {
    if (this.data === null || this.data === undefined) {
      return false;
    } else if(this.data.status === 0) {
      return true;
    } else {
      return false;
    }
  }

  onLogsClick() {
    this.signalRService.getLogs().subscribe((data: any) => {
      this.log = data.log.split('|').reverse();
      this.log.shift();
    }, err => {
      alert('Error occured while geting the logs!');
    });
  }

}
