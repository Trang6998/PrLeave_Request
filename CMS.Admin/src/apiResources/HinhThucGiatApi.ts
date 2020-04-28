import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { HinhThucGiat } from '@/models/HinhThucGiat'
export interface HinhThucGiatApiSearchParams extends Pagination {
    keywords?: string;
}
class HinhThucGiatApi extends BaseApi {
    search(searchParams: HinhThucGiatApiSearchParams): Promise<PaginatedResponse<HinhThucGiat>> {

        return new Promise<PaginatedResponse<HinhThucGiat>>((resolve: any, reject: any) => {
            HTTP.get('hinhthucgiat', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<HinhThucGiat> {
        return new Promise<HinhThucGiat>((resolve: any, reject: any) => {
            HTTP.get('hinhthucgiat/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, hinhThucGiat: HinhThucGiat): Promise<HinhThucGiat> {
        return new Promise<HinhThucGiat>((resolve: any, reject: any) => {
            HTTP.put('hinhthucgiat/' + id, 
                hinhThucGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(hinhThucGiat: HinhThucGiat): Promise<HinhThucGiat> {
        return new Promise<HinhThucGiat>((resolve: any, reject: any) => {
            HTTP.post('hinhthucgiat', 
                hinhThucGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<HinhThucGiat> {
        return new Promise<HinhThucGiat>((resolve: any, reject: any) => {
            HTTP.delete('hinhthucgiat/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new HinhThucGiatApi();
