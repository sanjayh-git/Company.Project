import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { MeterReadingsListComponent } from './meter-readings-list/meter-readings-list.component';
import { UploadMeterReadingsComponent } from './upload-meter-readings/upload-meter-readings.component';

@NgModule({
  declarations: [
    AppComponent,
    AccountsListComponent,
    MeterReadingsListComponent,
    UploadMeterReadingsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
