import {Directive, ElementRef, EventEmitter, Output, NgZone} from '@angular/core';
import {NgModel} from '@angular/forms';
import { MapsAPILoader } from '@agm/core';

declare var google:any;

@Directive({
  selector: '[googleplace]',
  providers: [NgModel],
  host: {
    '(input)' : 'onInputChange()'
  }
})

export class GoogleplaceDirective  {
  @Output() setAddress: EventEmitter<any> = new EventEmitter();
  modelValue:any;
  autocomplete:any;
  private _el:HTMLElement;

  constructor(el: ElementRef,private model:NgModel,
    private ngZone: NgZone,
    private mapsAPILoader: MapsAPILoader) {
    this._el = el.nativeElement;
    this.modelValue = this.model;
    var input = this._el;
    var options = {
      componentRestrictions: {country: "vn"},
      // types: ["address"]
    };
    this.mapsAPILoader.load().then(() => {
      let autocomplete = new google.maps.places.Autocomplete(this._el, options);
      autocomplete.addListener("place_changed", () => {
        this.ngZone.run(() => {
          //get the place result
          let place: google.maps.places.PlaceResult = autocomplete.getPlace();
          this.invokeEvent(place);
        });
      });
    });
  }

  invokeEvent(place:Object) {
    this.setAddress.emit(place);
  }

  onInputChange() {
  }
}