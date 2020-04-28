import { DoGiat } from "@/models/DoGiat";
import { HinhThucGiat } from "@/models/HinhThucGiat";
import { NguoiDung } from "@/models/NguoiDung";

export interface DonGia {
    DonGiaID: number;
    HinhThucGiatID: number;
    DoGiatID: number;
    DonGiaGiat: number;
    GhiChu: string;
    DoGiat?: DoGiat;
    HinhThucGiat?: HinhThucGiat;
    NguoiDung?: NguoiDung;
}
