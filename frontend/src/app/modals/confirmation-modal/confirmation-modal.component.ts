import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { ConfirmationModalService } from '../../services/confirmation-modal.service';
import { ConfirmationConfigModal } from '../../models/confirmation-config.model';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirmation-modal.component.html',
  styleUrl: './confirmation-modal.component.css'
})
export class ConfirmationModalComponent implements OnInit, OnDestroy {
  private confirmModalService = inject(ConfirmationModalService);
  
  showModal = false;

  config: ConfirmationConfigModal = {
    title: 'Confirmação',
    message: 'Tem certeza que deseja realizar esta ação?',
    confirmText: 'Sim',
    cancelText: 'Não',
    type: 'warning'
  };

  private subscriptions: Subscription[] = [];

  ngOnInit() {
    this.subscriptions.push(
      this.confirmModalService.getShowModal().subscribe(show => {
        this.showModal = show;
      }),
      
      this.confirmModalService.getModalConfig().subscribe(config => {
        this.config = { ...this.config, ...config };
      })
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  onConfirm() {
    this.confirmModalService.confirm();
  }

  onCancel() {
    this.confirmModalService.cancel();
  }

  onClose() {
    this.confirmModalService.close();
  }

  getIconClass(): string {
    switch (this.config.type) {
      case 'danger': return 'fas fa-exclamation-triangle';
      case 'warning': return 'fas fa-exclamation-circle';
      case 'success': return 'fas fa-check-circle';
      case 'info': return 'fas fa-info-circle';
      default: return 'fas fa-question-circle';
    }
  }

  getIconColor(): string {
    switch (this.config.type) {
      case 'danger': return 'var(--danger-color)';
      case 'warning': return 'var(--warning-color)';
      case 'success': return 'var(--success-color)';
      case 'info': return 'var(--primary-color)';
      default: return 'var(--warning-color)';
    }
  }

  getConfirmButtonClass(): string {
    switch (this.config.type) {
      case 'danger': return 'btn-danger';
      case 'warning': return 'btn-warning';
      case 'success': return 'btn-success';
      case 'info': return 'btn-primary';
      default: return 'btn-warning';
    }
  }
}