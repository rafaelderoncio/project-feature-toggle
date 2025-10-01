import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LogService } from '../../services/log.service';
import { Log } from '../../models/log.model';

@Component({
  selector: 'app-log-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './log.page.html',
  styleUrl: './log.page.css'
})
export class LogPageComponent {
  logService = inject(LogService);

  logs = this.logService.getLogs();
  searchQuery = signal('');
  currentLogFilter = signal<'all' | 'today' | 'week'>('all');

  get filteredLogs() {
    return this.logs().filter(log => {
      // Filtro de busca
      const matchesSearch = log.message.toLowerCase().includes(this.searchQuery().toLowerCase());
      
      // Filtro de tempo
      const now = new Date();
      let matchesTimeFilter = true;
      
      if (this.currentLogFilter() === 'today') {
        const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        matchesTimeFilter = log.timestamp >= today;
      } else if (this.currentLogFilter() === 'week') {
        const oneWeekAgo = new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000);
        matchesTimeFilter = log.timestamp >= oneWeekAgo;
      }
      
      return matchesSearch && matchesTimeFilter;
    }).sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());
  }

  onSearchChange(event: Event) {
    this.searchQuery.set((event.target as HTMLInputElement).value.toLowerCase());
  }

  setLogFilter(filter: 'all' | 'today' | 'week') {
    this.currentLogFilter.set(filter);
  }

  formatTime(timestamp: Date) {
    const now = new Date();
    const diff = now.getTime() - timestamp.getTime();
    
    if (diff < 60000) { // Menos de 1 minuto
      return 'Agora mesmo';
    } else if (diff < 3600000) { // Menos de 1 hora
      const minutes = Math.floor(diff / 60000);
      return `Há ${minutes} minuto${minutes > 1 ? 's' : ''}`;
    } else if (diff < 86400000) { // Menos de 1 dia
      const hours = Math.floor(diff / 3600000);
      return `Há ${hours} hora${hours > 1 ? 's' : ''}`;
    } else {
      const days = Math.floor(diff / 86400000);
      if (days === 1) return 'Ontem';
      return `Há ${days} dias`;
    }
  }
}