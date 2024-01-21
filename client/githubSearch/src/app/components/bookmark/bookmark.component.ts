import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.component.html',
  styleUrls: ['./bookmark.component.scss']
})
export class BookmarkComponent implements OnInit {

  bookMarks: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getData();
  }

  getData(){
    this.http.get(`${environment.apiEndpoint}/bookmark/me`).subscribe((e: any) => this.bookMarks = e);
  }

  bookMark(repo: any) {
    this.http.post(`${environment.apiEndpoint}/bookmark`, repo).subscribe((e: any) => {
      this.getData();
    }, e => console.log(e));
  }

}
