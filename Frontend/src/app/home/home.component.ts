import { Component, OnInit } from '@angular/core';
import { SwiperOptions } from 'swiper';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  show: number = 1;
  config: SwiperOptions = {
    pagination: { 
      el: '.swiper-pagination', 
      clickable: true 
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev'
    },
    spaceBetween: 30
  };
  constructor() {
    //setInterval(() => this.toggleImages(), 5000);
  }



  ngOnInit() {
  }

  public onMapReady(event: Event) {
    console.log("Map Ready")
  }

  toggleImages() {
    if(this.show == 1){
      this.show = 0 ;
    }
    else{
      this.show = 1;
    }
  }
}
