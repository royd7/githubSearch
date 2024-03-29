import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {

  query: string = '';
  searchRes: any;

  constructor(private http: HttpClient) {

  }

  //search repositories in github api
  searchRepos() {
    this.http.get(`${environment.apiEndpoint}/search?query=${this.query}`).subscribe((e: any) => {
      this.searchRes = e;
    }, (e) => console.log(e));

  }

  //post bookmark state to api
  bookMark(repo: any) {
    this.http.post(`${environment.apiEndpoint}/bookmark`, repo).subscribe((e: any) => {
      repo.isBookMark = e;
    }, e => console.log(e));
  }

}
