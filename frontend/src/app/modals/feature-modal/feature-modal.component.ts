import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Feature } from '../../models/feature.model';

@Component({
  selector: 'app-feature-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './feature-modal.component.html',
  styleUrl: './feature-modal.component.css'
})
export class FeatureModalComponent {
  @Input() feature: Feature | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<Feature>();

  name = '';
  description = '';
  enabled = true;

  ngOnInit() {
    if (this.feature) {
      this.name = this.feature.name;
      this.description = this.feature.description;
      this.enabled = this.feature.enabled;
    }
  }

  onSave() {
    if (!this.name.trim() || !this.description.trim()) return;

    const featureData: Feature = {
      id: this.feature?.id || Date.now().toString(),
      name: this.name,
      description: this.description,
      enabled: this.enabled
    };

    this.save.emit(featureData);
  }

  onClose() {
    this.close.emit();
  }
}