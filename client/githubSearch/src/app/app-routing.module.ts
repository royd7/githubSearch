import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from 'src/app/components/search/search.component';
import { AuthGuard } from './services/auth.guard.service';
import { BookmarkComponent } from './components/bookmark/bookmark.component';

const routes: Routes = [
  { path: "repos", component: SearchComponent, canActivate: [AuthGuard] },
  { path: "bookmark", component: BookmarkComponent, canActivate: [AuthGuard] },
  { path: "", redirectTo: "repos", pathMatch: 'full' },
  { path: "**", redirectTo: "", pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
