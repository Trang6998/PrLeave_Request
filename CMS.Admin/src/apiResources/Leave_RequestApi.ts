import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse, Pagination } from './PaginatedResponse'
import { Leave_Request } from '../models/Leave_Request';
export interface Leave_RequestApiSearchParams extends Pagination {
}
class Leave_RequestApi extends BaseApi {
    search(searchParams: Leave_RequestApiSearchParams): Promise<PaginatedResponse<Leave_Request>> {

        return new Promise<PaginatedResponse<Leave_Request>>((resolve: any, reject: any) => {
            HTTP.get('leave_request', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<Leave_Request> {
        return new Promise<Leave_Request>((resolve: any, reject: any) => {
            HTTP.get('leave_request/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, albumAnh: Leave_Request): Promise<Leave_Request> {
        return new Promise<Leave_Request>((resolve: any, reject: any) => {
            HTTP.put('leave_request/' + id,
                albumAnh
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(albumAnh: Leave_Request): Promise<Leave_Request> {
        return new Promise<Leave_Request>((resolve: any, reject: any) => {
            HTTP.post('leave_request',
                albumAnh
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<Leave_Request> {
        return new Promise<Leave_Request>((resolve: any, reject: any) => {
            HTTP.delete('leave_request/' + id)
                .then((response) => {
                    resolve(response.data);
                }).catch((error) => {
                    reject(error);
                })
        });
    }
}
export default new Leave_RequestApi();
