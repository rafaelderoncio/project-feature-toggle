import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FeatureService } from '../../services/feature.service';
import { LogService } from '../../services/log.service';
import { Feature } from '../../models/feature.model';
import { FeatureModalComponent } from '../../modals/feature-modal/feature-modal.component';
import { ToastrService } from '../../services/toastr.service';
import { ConfirmationModalService } from '../../services/confirmation-modal.service';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, FormsModule, FeatureModalComponent],
  templateUrl: './home.page.html',
  styleUrl: './home.page.css'
})
export class HomePageComponent {
  private featureService = inject(FeatureService);
  private logService = inject(LogService);
  private toastrService = inject(ToastrService);
  private confirmationModalService = inject(ConfirmationModalService);

  features = this.featureService.getFeatures();
  searchQuery = signal('');
  currentFilter = signal<'all' | 'active' | 'inactive'>('all');
  showModal = signal(false);
  editingFeature: Feature | null = null;

  get activeCount() {
    return this.features().filter(f => f.enabled).length;
  }

  get inactiveCount() {
    return this.features().filter(f => !f.enabled).length;
  }

  get totalCount() {
    return this.features().length;
  }

  get filteredFeatures() {
    return this.features().filter(feature => {
      const matchesSearch = feature.name.toLowerCase().includes(this.searchQuery().toLowerCase()) || 
                           feature.description.toLowerCase().includes(this.searchQuery().toLowerCase());
      
      let matchesFilter = true;
      if (this.currentFilter() === 'active') {
        matchesFilter = feature.enabled;
      } else if (this.currentFilter() === 'inactive') {
        matchesFilter = !feature.enabled;
      }
      
      return matchesSearch && matchesFilter;
    });
  }

  onSearchChange(event: Event) {
    this.searchQuery.set((event.target as HTMLInputElement).value.toLowerCase());
  }

  setFilter(filter: 'all' | 'active' | 'inactive') {
    this.currentFilter.set(filter);
  }

  openAddModal() {
    this.editingFeature = null;
    this.showModal.set(true);
  }

  openEditModal(feature: Feature) {
    this.editingFeature = feature;
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.editingFeature = null;
  }

  saveFeature(feature: Feature) {
    if (this.editingFeature) {
      this.featureService.updateFeature(feature.id, feature);
      this.logService.addLog(`Feature ${feature.name} atualizada por Rafael`);
    } else {
      this.featureService.addFeature(feature);
      this.logService.addLog(`Feature ${feature.name} adicionada por Rafael`);
    }
    this.closeModal();
  }

  toggleFeature(id: string) {
    this.featureService.toggleFeature(id);
    const feature = this.features().find(f => f.id === id);
    if (feature) {
      this.logService.addLog(`Feature ${feature.name} ${feature.enabled ? 'ativada' : 'desativada'} por Rafael`);
    }
  }

  deleteFeature(id: string) {
    const feature = this.features().find(f => f.id === id);

    if (feature) {
      this.confirmationModalService.show({
        title: 'Deletar Feature',
        message: `Certeza que deseja excluir a feature '${feature.name}'?`,
      }).subscribe(confirm => {
        if (confirm) {
          this.featureService.deleteFeature(id);
          this.logService.addLog(`Feature ${feature.name} excluída por Rafael`);
          this.toastrService.show('Deletar Feature', `Feature ${feature.name} deletada`, 'info')
        }
      });
    }
  }
}