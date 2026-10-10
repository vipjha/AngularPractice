import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Nvabar } from "./nvabar/nvabar";
import { Footer } from "./footer/footer";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [RouterOutlet, Nvabar, Footer]
})
export class App {
  protected readonly title = signal('client');
}
