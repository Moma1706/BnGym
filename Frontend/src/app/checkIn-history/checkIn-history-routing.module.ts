import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CheckInHistoryComponent } from "./checkIn-history.component";
import { ViewCheckInsByDateComponent } from "./view-checkIns-by-date/view-checkIns-by-date.component";

const routes: Routes = [
    {
        path: '', component: CheckInHistoryComponent,
        children: [
            { path: 'view-checkins-by-date', component: ViewCheckInsByDateComponent },
            
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CheckInHistoryRoutingModule { }