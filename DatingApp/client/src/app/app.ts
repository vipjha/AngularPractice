import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private  http= inject(HttpClient);
  protected readonly title = signal('client');
  protected readonly titleNew = 'clientNew';
  protected members=signal<any>([]);

  //constructor(private http:HttpClient) {
  //}

  async ngOnInit(){
      this.members.set(await this.getMembers());
  }

  async getMembers() {
    try {
      return lastValueFrom(this.http.get('http://localhost:5001/api/members'));
    } catch (error) {
      console.error('Error fetching members:', error);
      //return [];
      throw error;
    }
  }
}
