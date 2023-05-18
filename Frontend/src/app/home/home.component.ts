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

  imageObject: Array<object> = [
    {
      image: 'assets/img/teretana-slika-1.jpg',
      alt: 'Slika 1',
      thumbImage: 'assets/img/teretana-slika-1.jpg',
    },
    {
      image: 'assets/img/teretana-slika-2.jpg',
      alt: 'Slika 2',
      thumbImage: 'assets/img/teretana-slika-2.jpg',
    },
    {
      image: 'assets/img/teretana-slika-3.jpg',
      alt: 'Slika 3',
      thumbImage: 'assets/img/teretana-slika-3.jpg',
    },
    {
      image: 'assets/img/teretana-slika-4.jpg',
      alt: 'Slika 4',
      thumbImage: 'assets/img/teretana-slika-4.jpg',
    },
    {
      image: 'assets/img/teretana-slika-5.jpg',
      alt: 'Slika 5',
      thumbImage: 'assets/img/teretana-slika-5.jpg',
    },
    {
      image: 'assets/img/teretana-slika-6.jpg',
      alt: 'Slika ',
      thumbImage: 'assets/img/teretana-slika-6.jpg',
    },
    {
      image: 'assets/img/teretana-slika-7.jpg',
      alt: 'Slika ',
      thumbImage: 'assets/img/teretana-slika-7.jpg',
    }
];
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
