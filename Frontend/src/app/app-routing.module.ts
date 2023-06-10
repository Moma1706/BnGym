import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const accountModule = () => import('./account/account-module').then(x => x.AccountModule);
const gymWorkerModule = () => import('./gym-worker/gym-worker.module').then(x => x.GymWorkerModule);
const gymUserModule = () => import('./gym-user/gym-user.module').then(x => x.GymUserModule);
const dailyTrainingModule = () => import('./daily-training/daily-training.module').then(x => x.DailyTrainingModule);
const checkInHistoryModule = () => import('./checkIn-history/checkIn-history.module').then(x => x.CheckInHistoryModule);

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'account', loadChildren: accountModule },
  { path: 'gym-worker', loadChildren: gymWorkerModule },
  { path: 'gym-user', loadChildren: gymUserModule },
  { path: 'daily-training', loadChildren: dailyTrainingModule },
  { path: 'checkIn-history', loadChildren: checkInHistoryModule },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
