import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
    {
        path:'',
        redirectTo:'authentication',
        pathMatch: 'full'
    },

    {
        path: 'dashboard',
        loadChildren: () => import("./dashboard/dashboard.module").then(c => c.DashboardModule)
    },
    {
        path: 'authentication',
        loadChildren: () => import("./public/authentication/authentication.module").then(c => c.AuthenticationModule)
    },

];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

