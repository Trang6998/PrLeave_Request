import { DoGiat } from "@/models/DoGiat";
import { KhachDatHang } from "@/models/KhachDatHang";
import { HinhThucGiat } from "./HinhThucGiat";

export interface ChiTietDoGiat {
    ChiTietDoGiatID: number;
    KhachDatHangID: number;
    DoGiatID: number;
    HinhThucGiatID: number;
    SoLuong: number;
    DoGiat?: DoGiat;
    KhachDatHang?: KhachDatHang;
    HinhThucGiat?: HinhThucGiat;
}
