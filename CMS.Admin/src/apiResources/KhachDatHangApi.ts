import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { KhachDatHang } from '@/models/KhachDatHang'
export interface KhachDatHangApiSearchParams extends Pagination {
    nguoiDungID?: number;
    tuNgay?: Date;
    denNgay?: Date;
    keywords?: string;
    trangThai?: number;
    gioLayDo?: number;
    thanhToan?: number;
}
class KhachDatHangApi extends BaseApi {
    search(searchParams: KhachDatHangApiSearchParams): Promise<PaginatedResponse<KhachDatHang>> {

        return new Promise<PaginatedResponse<KhachDatHang>>((resolve: any, reject: any) => {
            HTTP.get('khachdathang', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<KhachDatHang> {
        return new Promise<KhachDatHang>((resolve: any, reject: any) => {
            HTTP.get('khachdathang/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, khachDatHang: KhachDatHang): Promise<KhachDatHang> {
        return new Promise<KhachDatHang>((resolve: any, reject: any) => {
            HTTP.put('khachdathang/' + id, 
                khachDatHang
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(khachDatHang: KhachDatHang): Promise<KhachDatHang> {
        return new Promise<KhachDatHang>((resolve: any, reject: any) => {
            HTTP.post('khachdathang', 
                khachDatHang
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<KhachDatHang> {
        return new Promise<KhachDatHang>((resolve: any, reject: any) => {
            HTTP.delete('khachdathang/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new KhachDatHangApi();
