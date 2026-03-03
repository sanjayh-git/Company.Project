import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { MeterReadingsListComponent } from './meter-readings-list/meter-readings-list.component';
import { UploadMeterReadingsComponent } from './upload-meter-readings/upload-meter-readings.component';

const routes: Routes = [
  { path: '', redirectTo: 'accounts', pathMatch: 'full' },
  { path: 'accounts', component: AccountsListComponent },
  { path: 'meter-readings', component: MeterReadingsListComponent },
  { path: 'upload', component: UploadMeterReadingsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
