import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { LoaiDoGiat } from '@/models/LoaiDoGiat'
export interface LoaiDoGiatApiSearchParams extends Pagination {
    keywords?: string;
}
class LoaiDoGiatApi extends BaseApi {
    search(searchParams: LoaiDoGiatApiSearchParams): Promise<PaginatedResponse<LoaiDoGiat>> {

        return new Promise<PaginatedResponse<LoaiDoGiat>>((resolve: any, reject: any) => {
            HTTP.get('loaidogiat', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<LoaiDoGiat> {
        return new Promise<LoaiDoGiat>((resolve: any, reject: any) => {
            HTTP.get('loaidogiat/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, loaiDoGiat: LoaiDoGiat): Promise<LoaiDoGiat> {
        return new Promise<LoaiDoGiat>((resolve: any, reject: any) => {
            HTTP.put('loaidogiat/' + id, 
                loaiDoGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(loaiDoGiat: LoaiDoGiat): Promise<LoaiDoGiat> {
        return new Promise<LoaiDoGiat>((resolve: any, reject: any) => {
            HTTP.post('loaidogiat', 
                loaiDoGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<LoaiDoGiat> {
        return new Promise<LoaiDoGiat>((resolve: any, reject: any) => {
            HTTP.delete('loaidogiat/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new LoaiDoGiatApi();
