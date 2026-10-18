import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => {
        // אם זו שגיאת שרת פנימית (500 ומעלה)
        if (err.status >= 500) {
          alert('שגיאה כללית בשרת');
        }
        
        // במקרה של סטטוס קוד 400 מעבירים את השגיאה הלאה לטיפול בתוך הקומפוננטה
        return throwError(() => err);
      })
    );
  }
}