import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse, Pagination } from './PaginatedResponse'
import { LuotTruyCap } from '@/models/LuotTruyCap'
export interface LuotTruyCapApiSearchParams extends Pagination {
    q?: string;
    fromThoiGian?: Date;
    toThoiGian?: Date;
}
class LuotTruyCapApi extends BaseApi {
    search(searchParams: LuotTruyCapApiSearchParams): Promise<PaginatedResponse<LuotTruyCap>> {

        return new Promise<PaginatedResponse<LuotTruyCap>>((resolve: any, reject: any) => {
            HTTP.get('luottruycap', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<LuotTruyCap> {
        return new Promise<LuotTruyCap>((resolve: any, reject: any) => {
            HTTP.get('luottruycap/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, luotTruyCap: LuotTruyCap): Promise<LuotTruyCap> {
        return new Promise<LuotTruyCap>((resolve: any, reject: any) => {
            HTTP.put('luottruycap/' + id,
                luotTruyCap
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(luotTruyCap: LuotTruyCap): Promise<LuotTruyCap> {
        return new Promise<LuotTruyCap>((resolve: any, reject: any) => {
            HTTP.post('luottruycap',
                luotTruyCap
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<LuotTruyCap> {
        return new Promise<LuotTruyCap>((resolve: any, reject: any) => {
            HTTP.delete('luottruycap/' + id)
                .then((response) => {
                    resolve(response.data);
                }).catch((error) => {
                    reject(error);
                })
        });
    }
}
export default new LuotTruyCapApi();
