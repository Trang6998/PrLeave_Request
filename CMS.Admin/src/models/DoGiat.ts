import { ChiTietDoGiat } from "@/models/ChiTietDoGiat";
import { DonGia } from "@/models/DonGia";
import { LoaiDoGiat } from "@/models/LoaiDoGiat";

export interface DoGiat {
    DoGiatID: number;
    LoaiDoGiatID: number;
    TenDoGiat: string;
    PCS: string;
    LoaiDoGiat?: LoaiDoGiat;
    ChiTietDoGiat?: ChiTietDoGiat[];
    DonGia?: DonGia[];
}
