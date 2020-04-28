import { KhachDatHang } from "@/models/KhachDatHang";
import { LienHe } from "@/models/LienHe";

export interface NguoiDung {
    NguoiDungID: number;
    TenNguoiDung: string;
    TenGoi: string;
    SoDienThoai: string;
    SoKhac: string;
    DiaChi: string;
    TaiKhoan: string;
    Facebook: string;
    Gmail: string;
    DiemThuong: number;
    KhachDatHang?: KhachDatHang[];
    LienHe?: LienHe[];
}
