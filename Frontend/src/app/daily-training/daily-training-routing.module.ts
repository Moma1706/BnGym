import { DailyTrainingComponent } from './daily-training.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddDailyTrainingComponent } from './add-daily-training/add-daily-training.component';
import { ViewAllDailyComponent } from './view-all-daily/view-all-daily.component';
import { ViewDailyUserComponent } from './view-daily-user/view-daily-user.component';

const routes: Routes = [
    {
        path: '', component: DailyTrainingComponent,
        children: [
            { path: 'add-daily-training', component: AddDailyTrainingComponent },
            { path: 'view-all-daily', component: ViewAllDailyComponent },
            { path: 'view-daily-user/:id', component: ViewDailyUserComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DailyTrainingRoutingModule { }