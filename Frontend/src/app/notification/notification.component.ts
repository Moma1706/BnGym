import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { Notification } from './interfaces/notification.interface';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit, OnChanges {

  @Input() notifications: Notification[] = [];
  @Input() notificationsExist: boolean = false;
  @Output() clearOneEvent = new EventEmitter<string>();
  notificationsArray: { message: string; status: string; dateTime: string; key: string}[] = [];

  constructor(private notificationService: NotificationService) {}

  ngOnChanges(): void {
    this.notifications.forEach(x => {
      if (!this.notificationsArray.find(y => y.key == x.key))
        this.notificationsArray.push(this.SplitValue(x));
    });
  }

  ngOnInit() {}

  ClearOne(id: string) {
    this.notificationService.deleteOne(id)
      .subscribe(() => {
        this.clearOneEvent.emit("clearOne");
        this.notificationsArray = this.notificationsArray.filter(item => item.key !== id);
      });
  }

  ClearAll() {
    this.notificationService.deleteAll()
      .subscribe(() => {
        this.clearOneEvent.emit("clearOne");
        this.notificationsArray = [];
      });
  }

  SplitValue(value: Notification): { message: string; status: string; dateTime: string; key: string;} {
    const splitStatus = value.value.split('Status:');
    const message: string = splitStatus[0].trim();

    const splitVrijeme = splitStatus[1].split('Vrijeme:');
    const status: string = splitVrijeme[0].trim();
    const vrijeme: string = splitVrijeme[1].trim();

    let novoVreme = Date.parse(vrijeme);

    var date = new Date(novoVreme);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();



    const newDate = new Date(Date.UTC(year, month, day, hours, minutes, seconds)).toLocaleString();
    return {
      message: message,
      status: status,
      dateTime: newDate,
      key: value.key
    }
  }
}
