import { DoGiat } from "@/models/DoGiat";

export interface LoaiDoGiat {
    LoaiDoGiatID: number;
    TenLoaiDoGiat: string;
    MoTa: string;
    DoGiat?: DoGiat[];
}
