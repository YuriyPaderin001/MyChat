import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AccountComponent } from './account.component';
import { ChatComponent } from './chat.component';
import { MessageComponent } from './message.component';

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule],
    declarations: [AppComponent, AccountComponent, ChatComponent, MessageComponent ],
    bootstrap: [AppComponent]
})

export class AppModule { }
