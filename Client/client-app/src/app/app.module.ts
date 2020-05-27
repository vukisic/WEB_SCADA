import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { TypePipe } from './pipes/typePipe.pipe';
import { AlarmPipe } from './pipes/alarmPipe.pipe';
import { TimestampPipe } from './pipes/timestampPipe.pipe';

@NgModule({
  declarations: [
    AppComponent,
    TypePipe,
    AlarmPipe,
    TimestampPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
