import { DayliTrainingComponent } from './dayli-training.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddDayliTrainingComponent } from './add-dayli-training/add-dayli-training.component';
import { ViewAllDayliComponent } from './view-all-dayli/view-all-dayli.component';
import { ViewDayliUserComponent } from './view-dayli-user/view-dayli-user.component';

const routes: Routes = [
    {
        path: '', component: DayliTrainingComponent,
        children: [
            { path: 'add-dayli-training', component: AddDayliTrainingComponent },
            { path: 'view-all-dayli', component: ViewAllDayliComponent },
            { path: 'view-daily-user/:id', component: ViewDayliUserComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DayliTrainingRoutingModule { }