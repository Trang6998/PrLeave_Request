import { NguoiDung } from "@/models/NguoiDung";

export interface LienHe {
    LienHeID: number;
    NguoiDungID: number;
    NoiDung: string;
    NguoiDung?: NguoiDung;
}