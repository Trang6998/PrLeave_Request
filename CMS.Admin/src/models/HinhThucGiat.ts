import { DonGia } from "@/models/DonGia";

export interface HinhThucGiat {
    HinhThucGiatID: number;
    TenHinhThuc: string;
    GhiChu: string;
    DonGia?: DonGia[];
}
