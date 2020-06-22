import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ResponseModel } from 'src/app/models/ResponseModel';
import { SignalRService } from 'src/app/services/signal-r.service';
import { Point } from 'src/app/models/Point';
import { PointType } from 'src/app/models/Enums';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

/*
  Main App Component - Client
*/
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  /// Response from server
  data: ResponseModel;
  log: string[] = [];
  /// Selected point
  selectedPoint: Point;
  constructor(public signalRService: SignalRService,
              private fb: FormBuilder,
              private http: HttpClient,
              private toastrService: ToastrService) { }

  /// Model for command form
  commandModel = this.fb.group({
    Value: [0],
    CurrentValue: [0],
    HighLimit: [0],
    LowLimit: [0],
    AbnormalValue: [0]
  });

  ngOnInit() {
    /// Register on server hub
    this.signalRService.startConnection();
    /// Add data listener
    this.signalRService.addDataListener();
    /// Send init request
    this.signalRService.startHttpRequest();
    /// Subscribe on data changes
    this.signalRService.dataSource$.subscribe(data => this.showData(data));
  }

  /// Function witch checks if data is valid and update selected point if exists
  /// Input: data eg. response form server;
  showData(data: ResponseModel) {
    if (data !== null && data !== undefined) {
      this.data = data;
      if (this.selectedPoint !== undefined) {
        data.list.forEach(x => {
          if (x.pointId === this.selectedPoint.pointId) {
            if (x.type === PointType.ANALOG_OUTPUT) {
              this.commandModel.patchValue({CurrentValue: x.eguValue});
            } else {
              this.commandModel.patchValue({CurrentValue: x.rawValue});
            }
          }
        });
      }
    }
  }

  /// Function witch checks and updates status of connection
  status() {
    if (this.data === null || this.data === undefined) {
      return false;
    } else if (this.data.status === 0) {
      return true;
    } else {
      return false;
    }
  }

  /// Functions that handle click on Log button
  /// Requests log data from server
  onLogsClick() {
    this.signalRService.hubConnection.invoke('logs').then(data => {
      this.log = data.split('|').reverse();
      this.log.shift();
    }).catch(() => {
      this.toastrService.error('Error occured while geting the logs!', 'Client');
    });
  }

  /// Function that checks if point is an input
  /// Input: point to be checkd
  /// Output: bool value
  isInput(model: Point) {
    return model.type === PointType.ANALOG_INPUT || model.type === PointType.DIGITAL_INPUT;
  }

  /// Function that handle click on command button and configure dialog based on point type
  /// Input: Point that was clicked
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

  /// Function that checks if command is valid eg. is commanded value in alarm scope
  isValidCommand() {
    const commandValue = this.commandModel.get('Value').value;
    if (this.selectedPoint !== undefined && this.selectedPoint.type === PointType.ANALOG_OUTPUT) {
      if (commandValue > this.commandModel.get('HighLimit').value ||
          commandValue < this.commandModel.get('LowLimit').value ||
          commandValue < this.selectedPoint.configItem.minValue ||
          commandValue > this.selectedPoint.configItem.maxValue) {
        return false;
      } else {
        return true;
      }
    } else {
      if (commandValue > 1 || commandValue < 0) {
        return false;
      } else {
        return true;
      }
    }
  }

  /// Function that sends command to server for execution, via hub connection
  /// Handle click of command button in command dialog
  onCommand() {
    if (this.status()) {
      if (this.isValidCommand()) {
        const pointId = this.selectedPoint.pointId;
        const address = this.selectedPoint.address;
        const value = this.commandModel.get('Value').value;
        this.signalRService.hubConnection.invoke('command', {pointId, address, value}).then(() => {
          this.selectedPoint = undefined;
          this.commandModel.reset();
          this.toastrService.success('Succesfully executed command!', 'Client');
        }).catch(() => {
          this.toastrService.error('Error occures  while execting command!', 'Client');
        });
      } else {
        this.toastrService.error('Command is not valid!', 'Client');
      }
    } else {
      this.toastrService.error('Disconnected!', 'Client');
    }
  }
}
