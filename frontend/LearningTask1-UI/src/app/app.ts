import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BusinessCardList } from "./components/business-card-list/business-card-list";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('LearningTask1-UI');
}
