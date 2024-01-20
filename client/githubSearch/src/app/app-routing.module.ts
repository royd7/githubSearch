import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from 'src/app/search/search.component';
import { AuthGuard } from './services/auth.guard.service';

const routes: Routes = [
  { path: "repos", component: SearchComponent, canActivate: [AuthGuard] },
  { path: "", redirectTo: "repos", pathMatch: 'full' },
  { path: "**", redirectTo: "", pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
