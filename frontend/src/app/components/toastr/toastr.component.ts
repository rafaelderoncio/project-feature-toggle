import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrService } from '../../services/toastr.service';

@Component({
  selector: 'app-toastr',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toastr.component.html',
  styleUrl: './toastr.component.css'
})
export class ToastrComponent {

  toastrService = inject(ToastrService);
  
  toasts = this.toastrService.getToasts();
  
  isDarkMode = signal(false);

  constructor() {
    this.checkDarkMode();
    
    const observer = new MutationObserver(() => {
      this.checkDarkMode();
    });
    
    observer.observe(document.body, {
      attributes: true,
      attributeFilter: ['class']
    });
  }

  private checkDarkMode() {
    this.isDarkMode.set(document.body.classList.contains('dark-mode'));
  }

  removeToast(id: string) {
    this.toastrService.remove(id);
  }

  getToastIcon(type: string): string {
    switch (type) {
      case 'success': return 'fas fa-check-circle';
      case 'error': return 'fas fa-exclamation-circle';
      case 'warning': return 'fas fa-exclamation-triangle';
      case 'info': return 'fas fa-info-circle';
      default: return 'fas fa-bell';
    }
  }
}