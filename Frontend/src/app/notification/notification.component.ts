import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NotificationService } from '../_services/notification.service';

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
}
