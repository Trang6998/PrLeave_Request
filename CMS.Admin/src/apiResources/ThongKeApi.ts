import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'

export interface ThongKeTuanApiSearchParams extends Pagination {
    tuan: number;
    thang: number;
    nam: number;
}
export interface ThongKeNgayApiSearchParams extends Pagination {
    ngay: Date;
}
class ThongKeApi extends BaseApi {
    thongKeTuan(searchParams: ThongKeTuanApiSearchParams): Promise<any> {

        return new Promise<any>((resolve: any, reject: any) => {
            HTTP.get('thongke/tuan', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    thongKeNgay(searchParams: ThongKeNgayApiSearchParams): Promise<any> {

        return new Promise<any>((resolve: any, reject: any) => {
            HTTP.get('thongke/ngay', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new ThongKeApi();
