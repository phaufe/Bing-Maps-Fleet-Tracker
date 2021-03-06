import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { DataService } from '../core/data.service';
import { Location } from '../shared/location';
import { Point } from '../shared/point';

@Injectable()
export class LocationService {

  constructor(private dataSevrice: DataService) { }

  addLocation(location: Location): Observable<void> {
    return this.dataSevrice.post<Location>('locations', location);
  }

  getLocations(): Observable<Location[]> {
    return this.dataSevrice.get<Location>('locations');
  }

  getLocation(id: number): Observable<Location> {
    return this.dataSevrice.getSingle<Location>('locations', id);
  }

  updateLocation(location: Location): Observable<void> {
    return this.dataSevrice.put<Location>('locations', location.id, location, true);
  }

  getLocationAssetsCount(location: Location): Observable<Map<string, number>> {
    return this.dataSevrice.getSingleNoCache<Map<string, number>>( `locations/${location.id}/assetsCount`);
  }

  getLocationMap(locations: Location[]): Map<string, Location> {
      const map: Map<string, Location> = new Map();
      for (const location of locations) {
        const locationName = this.generateLocationName(location);
        map.set(locationName, location);
      }
      return map;
    }

    generateLocationName(location: Location) {
      if (location.name === 'Auto-Generated Location') {
        return 'Location ' + location.id;
      } else {
        return location.name;
      }
    }
}
