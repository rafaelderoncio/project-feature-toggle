import { Injectable, signal } from '@angular/core';
import { Log } from '../models/log.model';

@Injectable({
  providedIn: 'root'
})
export class LogService {
  private logs = signal<Log[]>([
    {
      id: '1',
      message: 'Feature Dark Mode ativada por Rafael',
      timestamp: new Date(Date.now() - 30 * 60000)
    },
    {
      id: '2',
      message: 'Feature Social Login desativada por Ana',
      timestamp: new Date(Date.now() - 120 * 60000)
    },
    {
      id: '3',
      message: 'Feature Checkout Redesign adicionada por Carlos',
      timestamp: new Date(Date.now() - 24 * 3600000)
    },
    {
      id: '4',
      message: 'Feature Product Recommendations modificada por Maria',
      timestamp: new Date(Date.now() - 3 * 24 * 3600000)
    },
    {
      id: '5',
      message: 'Sistema de notificações ativado por João',
      timestamp: new Date(Date.now() - 5 * 24 * 3600000)
    }
  ]);

  getLogs() {
    return this.logs.asReadonly();
  }

  addLog(message: string) {
    const newLog: Log = {
      id: Date.now().toString(),
      message,
      timestamp: new Date()
    };
    this.logs.update(logs => [newLog, ...logs].slice(0, 100));
  }
}