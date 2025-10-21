import { inject, Injectable } from "@angular/core"
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn:'any'
})
export class SpinnerService {
     private _count = 0;
  private _visible$ = new BehaviorSubject<boolean>(false);
  visible$ = this._visible$.asObservable();

  // optional small delay to avoid flashing for very fast requests
  private showDelay = 120; // ms
  private showTimer: any;

  show() {
    this._count++;
    if (this._count === 1) {
      // start delay
      this.showTimer = setTimeout(() => {
        this._visible$.next(true);
      }, this.showDelay);
    }
  }

  hide() {
    if (this._count <= 0) return;
    this._count--;
    if (this._count === 0) {
      clearTimeout(this.showTimer);
      this._visible$.next(false);
    }
  }

  reset() {
    this._count = 0;
    clearTimeout(this.showTimer);
    this._visible$.next(false);
  }

}
