import {Component, NgZone, AfterViewInit, Output, Input, EventEmitter } from '@angular/core';
import {View, Feature, Map } from 'ol';
import {fromLonLat} from 'ol/proj';
import { ScaleLine, defaults as DefaultControls} from 'ol/control';
import proj4 from 'proj4';
import VectorLayer from 'ol/layer/Vector';
import {register}  from 'ol/proj/proj4';
import {get as GetProjection} from 'ol/proj'
import {Extent} from 'ol/extent';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM';
import Point from 'ol/geom/Point';
import Style from 'ol/style/Style';
import Icon from 'ol/style/Icon';
import VectorSource from 'ol/source/Vector';


@Component({
  selector: 'app-open-layers-map',
  templateUrl: './open-layers-map.component.html',
  styleUrls: ['./open-layers-map.component.css']
})
export class OpenLayersMapComponent implements AfterViewInit {
  @Input() zoom!: number;
  view!: View;
  projection = GetProjection('EPSG:3857');
  extent: Extent = [-20026376.39, -20048966.10,20026376.39, 20048966.10];
  Map!: Map;
  @Output() mapReady = new EventEmitter<Map>();

  constructor(private zone: NgZone) {
   }


  ngAfterViewInit():void {
    if (! this.Map) {
      this.zone.runOutsideAngular(() => this.initMap())
    } 
    setTimeout(()=>this.mapReady.emit(this.Map));
  }

  private initMap(): void{
    proj4.defs("EPSG:3857","+proj=merc +a=6378137 +b=6378137 +lat_ts=19.2166658 +lon_0=44.749997 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs");
    register(proj4)
    this.projection!.setExtent(this.extent);
    this.view = new View({
      center: fromLonLat([19.4435, 44.7761]),
      zoom: this.zoom,
      projection: this.projection!,
    });
    this.Map = new Map({
        layers: [new TileLayer({
        source: new OSM({})
      })],
      target: 'map',
      view: this.view,
      controls: DefaultControls().extend([
        new ScaleLine({}),
      ]),
    });


    const markerFeature = new Feature({
      geometry: new Point(fromLonLat([19.4435, 44.7761])),
      name: 'Marker'
    });

    const iconStyle = new Style({
      image: new Icon({
        src: 'assets/img/marker.png',
        scale: 0.1
      })
    });

    markerFeature.setStyle(iconStyle);

    const vectorLayer = new VectorLayer({
      source: new VectorSource({
        features: [markerFeature]
      })
    });

    this.Map.addLayer(vectorLayer);
  }

}