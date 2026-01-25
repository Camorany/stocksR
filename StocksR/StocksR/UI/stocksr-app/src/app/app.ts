import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SignalRClient } from './signal-rclient';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  constructor(private signalRService: SignalRClient){}
  public title = signal("Waiting for stock info...");

  ngOnInit(){
    this.signalRService.hubConnection.on("StockValueUpdated", (stock) => {
      this.title.set(JSON.stringify(stock));
    });
  }
}
