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

  name: string = '';
  description: string = '';
  active: boolean = true;

  ngOnInit() {
    if (this.feature) {
      this.name = this.feature.name;
      this.description = this.feature.description;
      this.active = this.feature.active;
    }
  }

  onSave() {
    if (!this.name.trim() || !this.description.trim()) {
      return;
    }

    const feature: Feature = {
      id: this.feature?.id || '',
      name: this.name,
      description: this.description,
      active: this.active === true || this.active,
      tags: [],
      feature: ''
    };

    this.save.emit(feature);
  }

  onClose() {
    this.close.emit();
  }
}