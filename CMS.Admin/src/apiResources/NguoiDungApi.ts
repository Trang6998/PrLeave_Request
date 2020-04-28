import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { NguoiDung } from '@/models/NguoiDung'
export interface NguoiDungApiSearchParams extends Pagination {
    keywords?: string;
}
class NguoiDungApi extends BaseApi {
    search(searchParams: NguoiDungApiSearchParams): Promise<PaginatedResponse<NguoiDung>> {

        return new Promise<PaginatedResponse<NguoiDung>>((resolve: any, reject: any) => {
            HTTP.get('nguoidung', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<NguoiDung> {
        return new Promise<NguoiDung>((resolve: any, reject: any) => {
            HTTP.get('nguoidung/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, nguoiDung: NguoiDung): Promise<NguoiDung> {
        return new Promise<NguoiDung>((resolve: any, reject: any) => {
            HTTP.put('nguoidung/' + id, 
                nguoiDung
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(nguoiDung: NguoiDung): Promise<NguoiDung> {
        return new Promise<NguoiDung>((resolve: any, reject: any) => {
            HTTP.post('nguoidung', 
                nguoiDung
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<NguoiDung> {
        return new Promise<NguoiDung>((resolve: any, reject: any) => {
            HTTP.delete('nguoidung/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new NguoiDungApi();
