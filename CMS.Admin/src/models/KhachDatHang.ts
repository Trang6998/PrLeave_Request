import { ChiTietDoGiat } from "@/models/ChiTietDoGiat";
import { NguoiDung } from "@/models/NguoiDung";
import { Users } from "./Users";

export interface KhachDatHang {
    KhachDatHangID: number;
    NguoiDungID: number;
    NgayDat: Date;
    GioDat: number;
    GhiChu: string;
    ThanhTien: number;
    TinhTrangXuLy: number;
    HinhThucThanhToan: number;
    TinhTrangThanhToan: number;
    UserID: number;
    NguoiDung?: NguoiDung;
    Users?: Users;
    ChiTietDoGiat?: ChiTietDoGiat[];
}
