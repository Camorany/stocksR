import { Component, Inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SignalRClient } from './signal-rclient';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  signalrService: SignalRClient = Inject(SignalRClient);
  protected readonly title = signal('Hello World!');
}
