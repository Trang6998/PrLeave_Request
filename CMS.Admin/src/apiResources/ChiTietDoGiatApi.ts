import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { ChiTietDoGiat } from '@/models/ChiTietDoGiat'
export interface ChiTietDoGiatApiSearchParams extends Pagination {
    khachDatHangID?:number;
    doGiatID?: number;
    hinhThucGiatID?: number;
}
class ChiTietDoGiatApi extends BaseApi {
    search(searchParams: ChiTietDoGiatApiSearchParams): Promise<PaginatedResponse<ChiTietDoGiat>> {

        return new Promise<PaginatedResponse<ChiTietDoGiat>>((resolve: any, reject: any) => {
            HTTP.get('chitietdogiat', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<ChiTietDoGiat> {
        return new Promise<ChiTietDoGiat>((resolve: any, reject: any) => {
            HTTP.get('chitietdogiat/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, chiTietDoGiat: ChiTietDoGiat): Promise<ChiTietDoGiat> {
        return new Promise<ChiTietDoGiat>((resolve: any, reject: any) => {
            HTTP.put('chitietdogiat/' + id, 
                chiTietDoGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(chiTietDoGiat: ChiTietDoGiat): Promise<ChiTietDoGiat> {
        return new Promise<ChiTietDoGiat>((resolve: any, reject: any) => {
            HTTP.post('chitietdogiat', 
                chiTietDoGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<ChiTietDoGiat> {
        return new Promise<ChiTietDoGiat>((resolve: any, reject: any) => {
            HTTP.delete('chitietdogiat/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new ChiTietDoGiatApi();
