import { MsgService } from '../local/msg.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HandleError } from '../../components/infrastructure/handleError';
import { ResponseModel } from '../../models/entity/response.model';

// @Injectable()
export class BaseService extends HandleError {
    constructor(
        protected messageService: MsgService,
        protected httpClient: HttpClient,
        protected urlName: string,
        protected apiName: string
    ) {
        super(messageService);
    }

    async getCustomApi(apiMethod: string, params: HttpParams): Promise<ResponseModel> {
        return await this.httpClient.get<ResponseModel>(`${this.urlName}/${this.apiName}/${apiMethod}`, { params }).toPromise();
    }

    async postCustomApi(apiMethod: string, model: object): Promise<ResponseModel> {
        return await this.httpClient.post<ResponseModel>(`${this.urlName}/${this.apiName}/${apiMethod}`, model).toPromise();
    }

    // async getAll(apiMethod: string): Promise<ResponseModel> {
    //     return await this.httpClient.get<ResponseModel>(`${this.urlName}/${this.apiName}/${apiMethod}`).toPromise();
    // }
}
