import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(BrowserModule)
  ]
})
  .catch(err => console.error(err));
