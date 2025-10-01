// import "@fortawesome/fontawesome-free/css/all.min.css";

import { Component, Inject, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { FormsModule } from '@angular/forms';
import { filter } from 'rxjs';
import { ToastrComponent } from './components/toastr/toastr.component';
import { ConfirmationModalComponent } from './modals/confirmation-modal/confirmation-modal.component';

let components = [SidebarComponent, ToastrComponent, ConfirmationModalComponent];
let modules = [RouterOutlet, CommonModule, FormsModule]

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    ... components,
    ... modules
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);
  darkMode = signal(false);
  userDropdownOpen = signal(false);
  pageTitle = signal('Gerenciamento de Feature Toggles');

  private pageTitles: { [key: string]: string } = {
    '/': 'Gerenciamento de Feature Toggles',
    '/logs': 'Logs de Alterações',
    '/settings': 'Configurações do Sistema'
  };

  constructor() {
    // Só acessa localStorage se estiver no navegador
    if (isPlatformBrowser(this.platformId)) {
      const savedDarkMode = localStorage.getItem('darkMode') === 'true';
      this.darkMode.set(savedDarkMode);
      document.body.classList.toggle('dark-mode', savedDarkMode);
    }
  }

  ngOnInit() {
    // Apply dark mode if active
    if (this.darkMode()) {
      document.body.classList.add('dark-mode');
    }

    // Watch for route changes to update page title
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        const url = event.urlAfterRedirects || event.url;
        this.pageTitle.set(this.pageTitles[url] || 'Gerenciamento de Feature Toggles');
      });

    // Set initial title based on current route
    const currentUrl = this.router.url;
    this.pageTitle.set(this.pageTitles[currentUrl] || 'Gerenciamento de Feature Toggles');
  }

  toggleDarkMode() {
    this.darkMode.set(!this.darkMode());
    document.body.classList.toggle('dark-mode', this.darkMode());
    localStorage.setItem('darkMode', this.darkMode().toString());
  }

  toggleUserDropdown() {
    this.userDropdownOpen.set(!this.userDropdownOpen());
  }

  logout() {
    if (confirm('Tem certeza que deseja sair?')) {
      alert('Logout realizado com sucesso!');
      this.userDropdownOpen.set(false);
    }
  }
}