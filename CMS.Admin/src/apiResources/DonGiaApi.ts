import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { DonGia } from '@/models/DonGia'
export interface DonGiaApiSearchParams extends Pagination {
    hinhThucGiatID?:number;
    doGiatID?: number;
    giaTu?: number;
    giaDen?: number;
}
class DonGiaApi extends BaseApi {
    search(searchParams: DonGiaApiSearchParams): Promise<PaginatedResponse<DonGia>> {

        return new Promise<PaginatedResponse<DonGia>>((resolve: any, reject: any) => {
            HTTP.get('dongia', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<DonGia> {
        return new Promise<DonGia>((resolve: any, reject: any) => {
            HTTP.get('dongia/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, donGia: DonGia): Promise<DonGia> {
        return new Promise<DonGia>((resolve: any, reject: any) => {
            HTTP.put('dongia/' + id, 
                donGia
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(donGia: DonGia): Promise<DonGia> {
        return new Promise<DonGia>((resolve: any, reject: any) => {
            HTTP.post('dongia', 
                donGia
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<DonGia> {
        return new Promise<DonGia>((resolve: any, reject: any) => {
            HTTP.delete('dongia/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new DonGiaApi();
