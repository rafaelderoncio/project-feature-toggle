import { Position } from "../types/position-type";
import { Notification } from "../types/notification-type";

export interface ToastrModel {
  id: string;
  title: string;
  message: string;
  type: Notification;
  duration?: number;
  timestamp: Date;
}