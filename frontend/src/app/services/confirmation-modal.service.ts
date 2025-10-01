import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ConfirmationModal } from '../models/confirmation.model';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationModalService {
  private confirmSubject = new Subject<boolean>();
  private modalConfigSubject = new Subject<ConfirmationModal>();
  private showModalSubject = new Subject<boolean>();

  getModalConfig(): Observable<ConfirmationModal> {
    return this.modalConfigSubject.asObservable();
  }

  getShowModal(): Observable<boolean> {
    return this.showModalSubject.asObservable();
  }

  getResult(): Observable<boolean> {
    return this.confirmSubject.asObservable();
  }

  show(config: ConfirmationModal): Observable<boolean> {
    this.modalConfigSubject.next(config);
    this.showModalSubject.next(true);
    
    return this.confirmSubject.asObservable();
  }

  confirm() {
    this.confirmSubject.next(true);
    this.showModalSubject.next(false);
  }

  cancel() {
    this.confirmSubject.next(false);
    this.showModalSubject.next(false);
  }

  close() {
    this.showModalSubject.next(false);
  }
}