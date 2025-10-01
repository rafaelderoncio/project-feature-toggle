import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './settings.page.html',
  styleUrl: './settings.page.css'
})
export class SettingsPageComponent {
  darkMode = signal(localStorage.getItem('darkMode') === 'true');
  notifications = signal(true);
  emailNotifications = signal(true);
  pushNotifications = signal(false);
  twoFactorAuth = signal(false);
  accessLog = signal(true);
  autoBackup = signal(true);
  logsLimit = signal(false);

  constructor() {
    // Aplicar o modo escuro se estiver ativo
    this.applyDarkMode();
  }

  onDarkModeChange() {
    this.applyDarkMode();
    localStorage.setItem('darkMode', this.darkMode().toString());
  }

  private applyDarkMode() {
    if (this.darkMode()) {
      document.body.classList.add('dark-mode');
    } else {
      document.body.classList.remove('dark-mode');
    }
  }
}