import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { LoaiTaiKhoan } from '@/models/LoaiTaiKhoan'
export interface LoaiTaiKhoanApiSearchParams extends Pagination {
    loaiLoaiTaiKhoanID?: number;
    keywords?: string;
}
class LoaiTaiKhoanApi extends BaseApi {
    search(searchParams: LoaiTaiKhoanApiSearchParams): Promise<PaginatedResponse<LoaiTaiKhoan>> {

        return new Promise<PaginatedResponse<LoaiTaiKhoan>>((resolve: any, reject: any) => {
            HTTP.get('loaitaikhoan', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<LoaiTaiKhoan> {
        return new Promise<LoaiTaiKhoan>((resolve: any, reject: any) => {
            HTTP.get('loaitaikhoan/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, loaiTaiKhoan: LoaiTaiKhoan): Promise<LoaiTaiKhoan> {
        return new Promise<LoaiTaiKhoan>((resolve: any, reject: any) => {
            HTTP.put('loaitaikhoan/' + id, 
                loaiTaiKhoan
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(loaiTaiKhoan: LoaiTaiKhoan): Promise<LoaiTaiKhoan> {
        return new Promise<LoaiTaiKhoan>((resolve: any, reject: any) => {
            HTTP.post('loaitaikhoan', 
                loaiTaiKhoan
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<LoaiTaiKhoan> {
        return new Promise<LoaiTaiKhoan>((resolve: any, reject: any) => {
            HTTP.delete('loaitaikhoan/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new LoaiTaiKhoanApi();
