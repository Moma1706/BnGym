import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const accountModule = () => import('./account/account-module').then(x => x.AccountModule);
const gymWorkerModule = () => import('./gym-worker/gym-worker.module').then(x => x.GymWorkerModule);
const gymUserModule = () => import('./gym-user/gym-user.module').then(x => x.GymUserModule);

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'account', loadChildren: accountModule },
  { path: 'gym-worker', loadChildren: gymWorkerModule },
  { path: 'gym-user', loadChildren: gymUserModule },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
