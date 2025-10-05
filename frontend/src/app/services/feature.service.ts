import { inject, Injectable } from '@angular/core';
import { Feature } from '../models/feature.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Pagination } from '../models/pagination.model';
import { Dashboard } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class FeatureService {

  private http: HttpClient = inject(HttpClient);

  constructor() { }

  getFeatureDashboard() {
    const endpoint = environment.featureDashboardEndpoint;
    return this.http.get<Dashboard>(endpoint);
  }

  getFeatures(page: number, quantity: number = 6, search: string = '', filter: string = 'all') {
    const endpoint = environment.featureManagerEndpoint;
    const params = { page, quantity, search, filter };
    return this.http.get<Pagination>(endpoint, { params });
  }

  saveFeature(feature: Omit<Feature, 'id'>) {
    const endpoint = environment.featureManagerEndpoint;
    return this.http.post<Feature>(endpoint, feature);
  }

  updateFeature(id: string, feature: Partial<Feature>) {
    const endpoint = `${environment.featureManagerEndpoint}/${id}`;
    return this.http.put<Feature>(endpoint, feature);
  }

  deleteFeature(id: string) {
    const endpoint = `${environment.featureManagerEndpoint}/${id}`;
    return this.http.delete<Feature>(endpoint);
  }

  toggleFeature(feature: string) {
    const endpoint = `${environment.featureToggleEndpoint}/${feature}`;
    return this.http.put<boolean>(endpoint, null);
  }
}