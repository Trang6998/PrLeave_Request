import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse, Pagination } from './PaginatedResponse'
import { LienHe } from '@/models/LienHe'
export interface LienHeApiSearchParams extends Pagination {
    lienHeID?: number;
}
class LienHeApi extends BaseApi {
    search(searchParams: LienHeApiSearchParams): Promise<PaginatedResponse<LienHe>> {

        return new Promise<PaginatedResponse<LienHe>>((resolve: any, reject: any) => {
            HTTP.get('lienhe', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<LienHe> {
        return new Promise<LienHe>((resolve: any, reject: any) => {
            HTTP.get('lienhe/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, lienHe: LienHe): Promise<LienHe> {
        return new Promise<LienHe>((resolve: any, reject: any) => {
            HTTP.put('lienhe/' + id,
                lienHe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(lienHe: LienHe): Promise<LienHe> {
        return new Promise<LienHe>((resolve: any, reject: any) => {
            HTTP.post('lienhe',
                lienHe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<LienHe> {
        return new Promise<LienHe>((resolve: any, reject: any) => {
            HTTP.delete('lienhe/' + id)
                .then((response) => {
                    resolve(response.data);
                }).catch((error) => {
                    reject(error);
                })
        });
    }
}
export default new LienHeApi();
