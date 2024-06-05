import { Component, Input } from '@angular/core';
import { Building } from '../models/building.model';
import { BuildingService } from '../services/building.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-building-detail',
  templateUrl: './building-detail.component.html',
  styleUrls: ['./building-detail.component.css']
})
export class BuildingDetailComponent {
  @Input() building? : Building;

  constructor(private buildingService: BuildingService, private route: ActivatedRoute, private location: Location) {}

  ngOnInit(): void {
    this.getBuilding();
  }

  getBuilding(): void {
    /*const id = Number(this.route.snapshot.paramMap.get('id')); //Imagen estatica de la URL actual
    this.buildingService.getBuilding(id)
      .subscribe(x => this.building = x);
      */
  }

  /*
  goBack(): void {
    this.location.back();
  }

  save(): void {
    if (this.hero) {
      this.heroService.updateHero(this.hero)
        .subscribe(() => this.goBack());
    }
  }
  */
}
