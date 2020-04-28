import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { DoGiat } from '@/models/DoGiat'
export interface DoGiatApiSearchParams extends Pagination {
    loaiDoGiatID?: number;
    keywords?: string;
}
class DoGiatApi extends BaseApi {
    search(searchParams: DoGiatApiSearchParams): Promise<PaginatedResponse<DoGiat>> {

        return new Promise<PaginatedResponse<DoGiat>>((resolve: any, reject: any) => {
            HTTP.get('dogiat', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<DoGiat> {
        return new Promise<DoGiat>((resolve: any, reject: any) => {
            HTTP.get('dogiat/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, doGiat: DoGiat): Promise<DoGiat> {
        return new Promise<DoGiat>((resolve: any, reject: any) => {
            HTTP.put('dogiat/' + id, 
                doGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(doGiat: DoGiat): Promise<DoGiat> {
        return new Promise<DoGiat>((resolve: any, reject: any) => {
            HTTP.post('dogiat', 
                doGiat
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<DoGiat> {
        return new Promise<DoGiat>((resolve: any, reject: any) => {
            HTTP.delete('dogiat/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new DoGiatApi();
