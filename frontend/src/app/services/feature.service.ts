import { inject, Injectable, signal } from '@angular/core';
import { Feature } from '../models/feature.model';
import { ToastrService } from './toastr.service';

@Injectable({
  providedIn: 'root'
})
export class FeatureService {
  private features = signal<Feature[]>([
    {
      id: '1',
      name: 'Checkout Redesign',
      description: 'Nova interface para o processo de checkout',
      enabled: true
    },
    {
      id: '2',
      name: 'Social Login',
      description: 'Permitir login com redes sociais',
      enabled: false
    },
    {
      id: '3',
      name: 'Dark Mode',
      description: 'Implementação do modo escuro na aplicação',
      enabled: true
    },
    {
      id: '4',
      name: 'Product Recommendations',
      description: 'Sistema de recomendação de produtos',
      enabled: false
    }
  ]);

  private toastr = inject(ToastrService);

  constructor() { }

  getFeatures() {
    return this.features.asReadonly();
  }

  addFeature(feature: Omit<Feature, 'id'>) {
    const newFeature: Feature = {
      ...feature,
      id: Date.now().toString()
    };
    
    this.features.update(features => [...features, newFeature]);

    this.toastr.success('Nova Feature', 'Feature criada com sucesso!');
  }

  updateFeature(id: string, feature: Partial<Feature>) {
    this.features.update(features => 
      features.map(f => f.id === id ? { ...f, ...feature } : f)
    );

    this.toastr.success('Editar Feature', 'Feature atualizada com sucesso!');
  }

  deleteFeature(id: string) {
    this.features.update(features => features.filter(f => f.id !== id));

    this.toastr.success('Deletar Feature', 'Feature deletada com sucesso!');
  }

  toggleFeature(id: string) {
    this.features.update(features => 
      features.map(f => 
        f.id === id ? { ...f, enabled: !f.enabled } : f
      )
    );

    this.toastr.success('Toggle', 'Feature atualizada com sucesso!');
  }
}