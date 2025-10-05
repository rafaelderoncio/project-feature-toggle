import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FeatureService } from '../../services/feature.service';
import { LogService } from '../../services/log.service';
import { Feature } from '../../models/feature.model';
import { FeatureModalComponent } from '../../modals/feature-modal/feature-modal.component';
import { ToastrService } from '../../services/toastr.service';
import { ConfirmationModalService } from '../../services/confirmation-modal.service';
import { Dashboard } from '../../models/dashboard.model';
import { first } from 'rxjs';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, FormsModule, FeatureModalComponent],
  templateUrl: './home.page.html',
  styleUrl: './home.page.css'
})
export class HomePageComponent {

  // services
  private featureService = inject(FeatureService);
  private confirmationModalService = inject(ConfirmationModalService);
  private toastrService = inject(ToastrService);
  private logService = inject(LogService);

  // properties
  features = signal<Feature[]>([]);
  dashboard = signal<Dashboard>({ totalActives: 0, totalInactives: 0, totalFeatures: 0 });
  page = signal(1);
  totalPages = signal(1);
  searchQuery = signal('');
  currentFilter = signal<'all' | 'active' | 'inactive'>('all');
  showModal = signal(false);
  editingFeature: Feature | null = null;

  ngOnInit() {
    this.loadPage(1);
  }

  public loadPage(page: number) {

    this.featureService.getFeatureDashboard()
      .pipe(first())
      .subscribe({
        next: (resp => {
          this.dashboard.set(resp);
          this.featureService.getFeatures(page, undefined, this.searchQuery(), this.currentFilter())
          .subscribe({
            next: (resp) => {
              this.features.set(resp.items);
              this.page.set(resp.page);
              this.totalPages.set(resp.totalPages);
            }
          });
        })
      });
  }

  get filteredFeatures() {
    return this.features().filter(feature => {
      const matchesSearch = feature.name.toLowerCase().includes(this.searchQuery().toLowerCase());

      let matchesFilter = true;

      if (this.currentFilter() === 'active') {
        matchesFilter = feature.active;
      } else if (this.currentFilter() === 'inactive') {
        matchesFilter = !feature.active;
      }

      return matchesSearch && matchesFilter;
    });
  }

  public onSearchChange(event: Event) {    
    const search = (event.target as HTMLInputElement).value.toLowerCase();
    this.searchQuery.set(search);
    this.featureService.getFeatures(this.page(), undefined, search, this.currentFilter())
      .subscribe({
        next: (resp) => {
          this.features.set(resp.items);
          this.page.set(resp.page);
          this.totalPages.set(resp.totalPages);
        }
      });
  }

  public changePage(newPage: number) {
    if (newPage >= 1 && newPage <= this.totalPages()) {
      this.loadPage(newPage);
    }
  }

  public setFilter(filter: 'all' | 'active' | 'inactive') {
    this.currentFilter.set(filter);
    this.featureService.getFeatures(this.page(), undefined, undefined, this.currentFilter())
      .subscribe({
        next: (resp) => {
          this.features.set(resp.items);
          this.page.set(resp.page);
          this.totalPages.set(resp.totalPages);
        }
      });
  }

  public openAddModal() {
    this.editingFeature = null;
    this.showModal.set(true);
  }

  public openEditModal(feature: Feature) {
    this.editingFeature = feature;
    this.showModal.set(true);
  }

  public closeModal() {
    this.showModal.set(false);
    this.editingFeature = null;
  }

  public saveFeature(feature: Feature) {
    if (feature) {
      this.featureService.saveFeature(feature)
        .pipe(first())
        .subscribe({
          next: (() => {
            this.closeModal();
            this.toastrService.success('Toggle', `Feature ${feature.name} criada`);
            // TODO: log
            this.loadPage(this.page())
          }),
          error: (() => {
            this.closeModal();
            this.toastrService.error('Toggle', `Erro ao criar feature ${feature.name}`);
            // TODO: log
            this.loadPage(this.page())
          })
        });
    }
  }

  public updateFeature(feature: Feature) {
    if (feature) {
      this.featureService.updateFeature(feature.id, feature)
        .pipe(first())
        .subscribe({
          next: (resp => {
            this.closeModal();
            this.toastrService.success('Toggle', `Feature ${feature.name} atualizada`);
            // TODO: log
            this.loadPage(this.page())
          }),
          error: (() => {
            this.closeModal();
            this.toastrService.error('Toggle', `Erro ao atualizar da feature ${feature.name}`);
            // TODO: log
            this.loadPage(this.page())
          })
        });
    }
  }

  public deleteFeature(id: string) {

    const feature = this.features().find(f => f.id === id);

    if (feature) {
      this.confirmationModalService.show({
        title: 'Deletar Feature',
        message: `Certeza que deseja excluir a feature ${feature.name}?`,
      })
        .pipe(first())
        .subscribe(confirm => {
          if (confirm) {
            this.featureService.deleteFeature(feature.id).subscribe({
              next: (() => {
                this.toastrService.success('Deletar Feature', `Feature ${feature.name} deletada`);
                // TODO: log
                this.refresh()
              }),
              error: (() => {
                this.toastrService.error('Deletar Feature', `Erro ao deletar feature ${feature.name}`);
                // TODO: log
                this.refresh()
              })
            });
          }
        });
    }
  }

  public toggleFeature(id: string) {

    const feature = this.features().find(f => f.id === id);

    if (feature) {
      this.featureService.toggleFeature(feature.feature)
        .pipe(first())
        .subscribe({
          next: (() => {
            this.toastrService.success('Toggle', `Feature ${feature.name} ${!feature.active ? 'ativada' : 'desativada'}`);
            // TODO: log
            this.loadPage(this.page())
          }),
          error: (() => {
            this.toastrService.error('Toggle', `Erro ao alterar estado da feature ${feature.name}`);
            // TODO: log
            this.loadPage(this.page())
          })
        });
    }
  }

  private refresh() {
    if (this.page() > 1 && this.features().length == 1) {
      this.loadPage(this.page() - 1);
    } else {
      this.loadPage(this.page());
    }
  }
}