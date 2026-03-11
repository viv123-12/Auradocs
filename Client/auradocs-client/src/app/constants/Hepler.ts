import { HttpContext } from "@angular/common/http";
import { Observable } from "rxjs";

export class AuradocsHelpler{

    public static ApiCallHelper(EndpointFunctionCall:(arg?:any) => Observable<any>, onSuccess:() => void, onError:() => void, body?:any)
    {
        EndpointFunctionCall(body).subscribe({
          next:
            res => {
              if (res.status == 200){
                onSuccess();
              }
            },
          error:
            error => {
              console.error(error);
              onError();
            }
        });
    }

}