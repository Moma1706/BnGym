import { DayliTrainingComponent } from './dayli-training.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddDayliTrainingComponent } from './add-dayli-training/add-dayli-training.component';

const routes: Routes = [
    {
        path: '', component: DayliTrainingComponent,
        children: [
            { path: 'add-dayli-training', component: AddDayliTrainingComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DayliTrainingRoutingModule { }