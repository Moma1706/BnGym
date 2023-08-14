import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { Notification } from './interfaces/notification.interface';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {

  @Input() notifications: Notification[] = [];
  @Input() notificationsExist: boolean = false;
  @Output() clearOneEvent = new EventEmitter<string>();

  constructor(private notificationService: NotificationService) {}

  ngOnInit() {
  }

  ClearOne(id: string) {
    this.notificationService.deleteOne(id)
      .subscribe(() => {
        this.clearOneEvent.emit("clearOne");
      });
  }

  ClearAll() {
    this.notificationService.deleteAll()
      .subscribe(() => {});
  }

  SplitValue(value: string) {
    const splitStatus = value.split('Status:');
    const message: string = splitStatus[0].trim();

    const splitVrijeme = splitStatus[1].split('Vrijeme:');
    const status: string = splitVrijeme[0].trim();
    const vrijeme: string = splitVrijeme[1].trim();

    return {
      message: message,
      status: status,
      dateTime: vrijeme
    }
  }
}
