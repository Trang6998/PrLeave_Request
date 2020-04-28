import { Users } from "@/models/Users";

export interface CoSo {
    CoSoID: number;
    LoaiCoSo: number;
    TenCoSo: string;
    DiaChi: string;
    Users?: Users[];
}
