import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ResponseModel } from 'src/app/models/ResponseModel';
import { SignalRService } from 'src/app/services/signal-r.service';
import { Point } from 'src/app/models/Point';
import { PointType } from 'src/app/models/Enums';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  data: ResponseModel;
  log: string[] = [];
  selectedPoint: Point;
  constructor(public signalRService: SignalRService,
              private fb: FormBuilder,
              private http: HttpClient,
              private toastrService: ToastrService) { }

  commandModel = this.fb.group({
    Value: [0],
    CurrentValue: [0],
    HighLimit: [0],
    LowLimit: [0],
    AbnormalValue: [0]
  });

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
    } else if (this.data.status === 0) {
      return true;
    } else {
      return false;
    }
  }

  onLogsClick() {
    this.signalRService.hubConnection.invoke('logs').then(data => {
      this.log = data.split('|').reverse();
      this.log.shift();
    }).catch(() => {
      this.toastrService.error('Error occured while geting the logs!', 'Client');
    });
  }

  isInput(model: Point) {
    return model.type === PointType.ANALOG_INPUT || model.type === PointType.DIGITAL_INPUT;
  }

  onCommandPreview(item: Point) {
    this.selectedPoint = item;
    if (this.selectedPoint.type === PointType.ANALOG_OUTPUT) {
      this.commandModel.setValue({
        HighLimit: this.selectedPoint.configItem.highLimit,
        LowLimit: this.selectedPoint.configItem.lowLimit,
        CurrentValue: this.selectedPoint.eguValue,
        Value: 0,
        AbnormalValue: this.selectedPoint.configItem.abnormalValue
      });
    } else {
      this.commandModel.setValue({
        HighLimit: this.selectedPoint.configItem.highLimit,
        LowLimit: this.selectedPoint.configItem.lowLimit,
        CurrentValue: this.selectedPoint.rawValue,
        Value: 0,
        AbnormalValue: this.selectedPoint.configItem.abnormalValue
      });
    }
  }

  isValidCommand() {
    if (this.selectedPoint !== undefined && this.selectedPoint.type === PointType.ANALOG_OUTPUT) {
      const commandValue = this.commandModel.get('Value').value;
      if (commandValue >= this.commandModel.get('HighLimit').value ||
          commandValue <= this.commandModel.get('LowLimit').value ||
          commandValue < this.selectedPoint.configItem.minValue ||
          commandValue > this.selectedPoint.configItem.maxValue) {
        return false;
      } else {
        return true;
      }
    } else {
      return true;
    }
  }

  onCommand() {
    if (this.status()) {
      if (this.isValidCommand()) {
        const pointId = this.selectedPoint.pointId;
        const address = this.selectedPoint.address;
        const value = this.commandModel.get('Value').value;
        this.signalRService.hubConnection.invoke('command', {pointId, address, value}).then(() => {

        }).catch(() => {

        });
      } else {
        this.toastrService.error('Command is not valid!', 'Client');
      }
    } else {
      this.toastrService.error('Disconnected!', 'Client');
    }
  }
}
