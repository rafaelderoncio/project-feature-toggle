import { Injectable, signal } from '@angular/core';
import { ToastrModel } from '../models/toastr.model';

@Injectable({
  providedIn: 'root'
})
export class ToastrService {
  private toasts = signal<ToastrModel[]>([]);
  private defaultPosition: string = 'top-right';

  getToasts() {
    return this.toasts.asReadonly();
  }

  show(title: string, message: string, type: 'success' | 'error' | 'warning' | 'info' = 'info', duration: number = 5000) {
    const newToast: ToastrModel = {
      id: Date.now().toString(),
      title,
      message,
      type,
      duration,
      timestamp: new Date()
    };

    this.toasts.update(toasts => [...toasts, newToast]);

    if (duration > 0) {
      setTimeout(() => {
        this.remove(newToast.id);
      }, duration);
    }

    return newToast.id;
  }

  success(title: string, message: string, duration: number = 5000) {
    return this.show(title, message, 'success', duration);
  }

  error(title: string, message: string, duration: number = 5000) {
    return this.show(title, message, 'error', duration);
  }

  warning(title: string, message: string, duration: number = 5000) {
    return this.show(title, message, 'warning', duration);
  }

  info(title: string, message: string, duration: number = 5000) {
    return this.show(title, message, 'info', duration);
  }

  remove(id: string) {
    this.toasts.update(toasts => toasts.filter(toast => toast.id !== id));
  }

  clear() {
    this.toasts.set([]);
  }
}