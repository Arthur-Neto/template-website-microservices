/*@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) {}

    public intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err) => {
                if ([401, 403].indexOf(err.status) !== -1) {
                    this.authenticationService.logout();
                }

                const error = err.error;
                return throwError(error);
            })
        );
    }
}*/
