import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PermissionGuard } from "./core/guards/permission.guard";

const routes: Routes = [
    {
        path:'',
        redirectTo:'authentication',
        pathMatch: 'full'
    },

    {
        path: 'dashboard',
        canActivateChild: [PermissionGuard],
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

