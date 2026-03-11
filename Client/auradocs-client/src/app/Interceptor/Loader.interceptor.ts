import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn } from "@angular/common/http";
import { finalize, Observable } from "rxjs";
import { LoaderService } from "../services/loader-service";
import { inject } from "@angular/core";

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {

  const loaderService = inject(LoaderService);

  loaderService.show();

  return next(req).pipe(
    finalize(() => loaderService.hide())
  );
};