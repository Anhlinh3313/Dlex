import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { environment } from 'src/environments/environment';
import 'rxjs/add/operator/map';

@Injectable()
export class GeocodingApiService {
    API_KEY: string;
    API_URL: string;

    constructor(
        private http: Http
    ) {
        this.API_KEY = environment.gMapKey;
        this.API_URL = `https://maps.googleapis.com/maps/api/geocode/json?key=${this.API_KEY}`;
        // console.log(this.API_URL);
    }

    async findFromAddressAsync(address: string, postalCode?: string, place?: string, province?: string, region?: string, country?: string): Promise<google.maps.places.PlaceResult> {
        const response = await this.findFromAddress(address, postalCode, place, province, region, country).toPromise();
        if (response.status == "OK") {
            const data = response.results[0];
            let place = response.results[0] as google.maps.places.PlaceResult;
            place.geometry.location = new google.maps.LatLng(data.geometry.location.lat, data.geometry.location.lng);
            return place;
        }
        return null;
    }

    findFromAddress(address: string, postalCode?: string, place?: string, province?: string, region?: string, country?: string): Observable<any> {
        let compositeAddress = [address];

        if (postalCode) compositeAddress.push(postalCode);
        if (place) compositeAddress.push(place);
        if (province) compositeAddress.push(province);
        if (region) compositeAddress.push(region);
        if (country) compositeAddress.push(country);

        let url = `${this.API_URL}&address=${compositeAddress.join(',')}`;

        return this.http.get(url).map(response => <any>response.json());
    }

    async findFirstFromLatLngAsync(lat: any, lng: any): Promise<google.maps.places.PlaceResult> {
        const response = await this.findFromLatLng(lat, lng).toPromise();
        if (response.status == "OK") {
            // console.log(response.plus_code);
            // console.log(response.plus_code.compound_code);
            const compound_code = response.plus_code.compound_code;
            if (compound_code.indexOf("Việt Nam") != -1) {
                const results = response.results.sort((x, y) => y.address_components.length - x.address_components.length) as google.maps.places.PlaceResult[];
                const data = results[0];

                let findTypeWard = data.address_components.filter(x => x.types.find(y => y == "sublocality_level_1"))[0];
                if (!findTypeWard) {
                    for (const item in results) {
                        if (results.hasOwnProperty(item)) {
                            const element = results[item];
                            let typeWard = element.address_components.filter(x => x.types.find(y => y == "sublocality_level_1"))[0];
                            if (typeWard) {
                                data.address_components.push(typeWard);
                                break;
                            }
                        }
                    }
                }
                return data;
            } else {
                let rs: google.maps.places.PlaceResult = new Object() as google.maps.places.PlaceResult;
                return rs;
            }
        }
        return null;
    }

    findFromLatLng(lat: number, lng: number): Observable<any> {
        // console.log(this.API_URL);
        let compositeLatLng = [lat, lng];
        let url = `${this.API_URL}&latlng=${compositeLatLng.join(',')}`;

        return this.http.get(url).map(response => <any>response.json());
    }
}