import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home/home.page';
import { LogPageComponent } from './pages/logs/logs.page';
import { SettingsPageComponent } from './pages/settings/settings.page';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'logs', component: LogPageComponent },
  { path: 'settings', component: SettingsPageComponent },
  { path: '**', redirectTo: '' }
];