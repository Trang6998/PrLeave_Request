<template>
    <v-dialog v-model="isShow" width="800" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập đơn hàng' : 'Thêm mới đơn hàng' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formKhachDatHang">
                    <v-layout row wrap>
                        <v-flex xs12 md6>
                            <v-autocomplete v-model="khachDatHang.NguoiDungID"
                                            :readonly="checkReadOnly"
                                            :items="dsNguoiDung"
                                            item-text="TenNguoiDung"
                                            item-value="NguoiDungID"
                                            label="Tên người dùng"
                                            :error-messages="errors.collect('Tên người dùng', 'formKhachDatHang')"
                                            v-validate="'required'"
                                            data-vv-scope="formKhachDatHang"
                                            data-vv-name="Tên người dùng" class="mr-1 ml-1"
                                            required></v-autocomplete>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-select v-model="khachDatHang.HinhThucThanhToan"
                                      :readonly="checkReadOnly"
                                      :items="lstHinhThucThanhToan"
                                      item-text="tt"
                                      item-value="value"
                                      label="Hình thức thanh toán"
                                      :error-messages="errors.collect('Hình thức thanh toán', 'formKhachDatHang')"
                                      v-validate="'required'"
                                      data-vv-scope="formKhachDatHang"
                                      data-vv-name="Hình thức thanh toán" class="mr-1 ml-1"
                                      required></v-select>
                        </v-flex>
                        <v-flex xs6 md3>
                            <v-datepicker v-model="khachDatHang.NgayDat"
                                          :readonly="checkReadOnly"
                                          label="Ngày đặt"
                                          :counter="50"
                                          :error-messages="errors.collect('Ngày đặt', 'formKhachDatHang')"
                                          v-validate="'max:50'"
                                          data-vv-scope="formKhachDatHang" class="mr-1 ml-1"
                                          data-vv-name="Ngày đặt">
                            </v-datepicker>
                        </v-flex>
                        <v-flex xs6 md3>
                            <v-select v-model="khachDatHang.GioDat"
                                      :readonly="checkReadOnly"
                                      :items="lstGioDat"
                                      item-text="tt"
                                      item-value="value"
                                      label="Giờ đặt"
                                      :error-messages="errors.collect('Giờ đặt', 'formKhachDatHang')"
                                      v-validate="'max:50'"
                                      data-vv-scope="formKhachDatHang" class="mr-1 ml-1"
                                      data-vv-name="Giờ đặt">
                            </v-select>
                        </v-flex>
                        <v-flex xs12 md3>
                            <v-select v-model="khachDatHang.TinhTrangXuLy"
                                      :readonly="checkReadOnly"
                                      :items="lstTinhTrang"
                                      item-text="tt"
                                      item-value="value"
                                      label="Tình trạng"
                                      :error-messages="errors.collect('Tình trạng', 'formKhachDatHang')"
                                      v-validate="'required'"
                                      data-vv-scope="formKhachDatHang" class="mr-1 ml-1"
                                      data-vv-name="Tình trạng"
                                      required></v-select>
                        </v-flex>
                        <v-flex xs12 md3>
                            <v-select v-model="khachDatHang.TinhTrangThanhToan"
                                      :readonly="checkReadOnly"
                                      :items="lstTinhTrangThanhToan"
                                      item-text="tt"
                                      item-value="value"
                                      label="Tình trạng thanh toán"
                                      :error-messages="errors.collect('Tình trạng thanh toán', 'formKhachDatHang')"
                                      v-validate="'required'"
                                      data-vv-scope="formKhachDatHang"
                                      data-vv-name="Tình trạng thanh toán" class="mr-1 ml-1"
                                      required></v-select>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="khachDatHang.GhiChu"
                                          :readonly="checkReadOnly"
                                          label="Ghi chú"
                                          :counter="200"
                                          :error-messages="errors.collect('Ghi chú', 'formKhachDatHang')"
                                          data-vv-scope="formKhachDatHang" class="mr-1 ml-1"
                                          data-vv-name="Ghi chú">
                            </v-text-field>
                        </v-flex>
                    </v-layout>
                    <v-layout row wrap v-show="isUpdate">
                        <v-flex xs12>
                            <h2>Chi tiết đơn đặt hàng</h2>
                        </v-flex>
                        <v-layout row wrap>
                            <v-flex xs6 md4>
                                <v-autocomplete v-model="chiTietDoGiat.DoGiatID"
                                                :items="dsDoGiat"
                                                item-text="TenDoGiat"
                                                item-value="DoGiatID" class="mr-1 ml-1"
                                                label="Tên đồ giặt"
                                                :error-messages="errors.collect('Tên đồ giặt', 'formChiTietDoGiat')"
                                                v-validate="'required'"
                                                data-vv-scope="formChiTietDoGiat"
                                                data-vv-name="Tên đồ giặt"
                                                required></v-autocomplete>
                            </v-flex>
                            <v-flex xs6 md4>
                                <v-autocomplete v-model="chiTietDoGiat.HinhThucGiatID"
                                                :items="dsHinhThucGiat"
                                                item-text="TenHinhThuc"
                                                item-value="HinhThucGiatID" class="mr-1 ml-1"
                                                label="Hình thức giặt"
                                                :error-messages="errors.collect('Hình thức giặt', 'formChiTietDoGiat')"
                                                v-validate="'required'"
                                                data-vv-scope="formChiTietDoGiat"
                                                data-vv-name="Hình thức giặt"
                                                required></v-autocomplete>
                            </v-flex>
                            <v-flex xs6 md2>
                                <v-text-field v-model="chiTietDoGiat.SoLuong"
                                              label="Số lượng" class="mr-1 ml-1"
                                              type="number"
                                              :error-messages="errors.collect('Số lượng', 'formChiTietDoGiat')"
                                              v-validate="'required|numeric'"
                                              data-vv-scope="formChiTietDoGiat"
                                              data-vv-name="Số lượng">
                                </v-text-field>
                            </v-flex>
                            <v-flex xs6 md2>

                                <v-btn color="orange lighten-2" v-show="!checkReadOnly" style="float: right" fab dark small @click="reloadChiTietDoGiat()">
                                    <v-icon small class="px-0">autorenew</v-icon>
                                </v-btn>
                                <v-btn color="orange lighten-2" v-show="!checkReadOnly" style="float: right" fab dark small @click="saveChiTietDoGiat()">
                                    <v-icon small class="px-0">{{isUpdateChTiet == false ? 'add' :'save'}}</v-icon>
                                </v-btn>
                            </v-flex>
                        </v-layout>
                        <v-flex xs12>
                            <v-data-table :headers="tableHeader"
                                          :items="dsChiTietDoGiat"
                                          :loading="loadingTable"
                                          :search="search" hide-actions
                                          class="table-border table"
                                          >
                                <template slot="items" slot-scope="props" >
                                    <tr @click="selectedRow(props.item)" style="cursor: pointer">
                                        <td class="text-xs-center">
                                            <v-btn icon small class="mx-0" :disabled="checkReadOnly" @click="confirmDelete(props.item)">
                                                <v-icon small color="pink">delete</v-icon>
                                            </v-btn>
                                        </td>
                                        <td class="text-xs-center">{{ props.item.DoGiat.TenDoGiat }} ({{ props.item.SoLuong }})</td>
                                        <td class="text-xs-center">{{ props.item.HinhThucGiat.TenHinhThuc }}</td>
                                    </tr>
                                </template>
                            </v-data-table>
                        </v-flex>

                        <v-flex xs12 class="mb-2 mt-2">
                            <div class="headline" style="float: right; color: red">Tổng tiền: {{khachDatHang.ThanhTien}} VNĐ</div>
                        </v-flex>
                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions v-show="!checkReadOnly">
                <v-spacer></v-spacer>
                <v-btn text @click.native="hide">Hủy</v-btn>
                <v-btn text @click.native="save" color="teal lighten-2"
                       :loading="loadingSave">Lưu</v-btn>
            </v-card-actions>
        </v-card>
        <v-dialog v-model="dialogConfirmDelete" max-width="290">
            <v-card>
                <v-card-title class="headline">Xác nhận xóa</v-card-title>
                <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                    <v-btn color="red darken-1" @click.native="deleteChiTietDoGiat" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-dialog>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import KhachDatHangApi, { KhachDatHangApiSearchParams } from '@/apiResources/KhachDatHangApi';
    import { KhachDatHang } from '@/models/KhachDatHang';
    import { ChiTietDoGiat } from '@/models/ChiTietDoGiat';
    import ChiTietDoGiatApi, { ChiTietDoGiatApiSearchParams } from '@/apiResources/ChiTietDoGiatApi';
    import NguoiDungApi, { NguoiDungApiSearchParams } from '@/apiResources/NguoiDungApi';
    import { NguoiDung } from '@/models/NguoiDung';
    import ThemSuaChiTietDoGiat from './ThemSuaChiTietDoGiat.vue';
    import DoGiatApi, { DoGiatApiSearchParams } from '@/apiResources/DoGiatApi';
    import { DoGiat } from '@/models/DoGiat';
    import HinhThucGiatApi, { HinhThucGiatApiSearchParams } from '@/apiResources/HinhThucGiatApi';
    import { HinhThucGiat } from '@/models/HinhThucGiat';

    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {
            ThemSuaChiTietDoGiat
        },
        data() {
            return {
                dsHinhThucGiat: [] as HinhThucGiat[],
                dsDoGiat: [] as DoGiat[],
                chiTietDoGiat: {} as ChiTietDoGiat,
                loading: false,
                valid: false,
                isShow: false,
                loadingSave: false,
                searchParamsChiTietDoGiat: { includeEntities: true, rowsPerPage: 0 } as ChiTietDoGiatApiSearchParams,
                searchParamsHinhThucGiat: { includeEntities: true, rowsPerPage: 10 } as HinhThucGiatApiSearchParams,
                searchParamsDoGiat: { includeEntities: true, rowsPerPage: 10 } as DoGiatApiSearchParams,
                isUpdateChTiet: false,
                isUpdate: false,
                khachDatHang: { TinhTrangXuLy: 1, HinhThucThanhToan: 2, TinhTrangThanhToan: 2, GioDat: 1, NgayDat: new Date() } as KhachDatHang,
                dsChiTietDoGiat: [] as ChiTietDoGiat[],
                dsNguoiDung: [] as NguoiDung[],
                searchParamsNguoiDung: { includeEntities: true, rowsPerPage: 10 } as NguoiDungApiSearchParams,
                tableHeader: [
                    { text: '#', value: '#', align: 'center', sortable: false },
                    { text: 'Tên đồ giặt', value: 'DoGiat.TenDoGiat', align: 'center', sortable: true },
                    { text: 'Hình thức giặt', value: 'HinhThucGiat.TenHinhThuc', align: 'center', sortable: true },
                ],
                checkReadOnly: false,
                search: "",
                loadingTable: false,
                searchParamsKhachDatHang: { includeEntities: true, rowsPerPage: 0 } as KhachDatHangApiSearchParams,
                selectedChiTietDoGiat: {} as ChiTietDoGiat,
                dialogConfirmDelete: false,
                lstTinhTrang: [
                    { tt: 'Chưa xử lý', value: 1 },
                    { tt: 'Đang xử lý', value: 2 },
                    { tt: 'Đã xử lý', value: 3 }
                ],
                lstHinhThucThanhToan: [
                    { tt: 'Thanh toán trước', value: 1 },
                    { tt: 'Thanh toán sau', value: 2 },
                    { tt: 'Thanh toán qua thẻ', value: 3 }
                ],
                lstTinhTrangThanhToan: [
                    { tt: 'Đã thanh toán', value: 1 },
                    { tt: 'Chưa thanh toán', value: 2 },
                ],
                lstGioDat: [
                    { tt: '7h30 - 9h', value: 1 },
                    { tt: '18h - 20h30', value: 2 },
                ]
            }
        },
        watch: {
        },
        created() {
            this.getDanhSachNguoiDung();
            //this.getChiTietDoGiatFromApi(this.searchParamsChiTietDoGiat);
        },
        methods: {
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                this.getDanhSachHinhThucGiat();
                this.getDanhSachDoGiat();
                this.dsChiTietDoGiat = [] as ChiTietDoGiat[];
                if (isUpdate) {
                    this.getDataFromApi(item.KhachDatHangID);
                }
                else {
                    this.khachDatHang = { TinhTrangXuLy: 1, HinhThucThanhToan: 2, TinhTrangThanhToan: 2, GioDat: 1, NgayDat: new Date() } as KhachDatHang;
                }
            },
            getDanhSachNguoiDung(): void {
                NguoiDungApi.search(this.searchParamsNguoiDung).then(res => {
                    this.dsNguoiDung = res.Data;
                });
            },
            getDonDatHang(): void {
                KhachDatHangApi.detail(this.khachDatHang.KhachDatHangID).then(res => {
                    this.khachDatHang = res;
                });
            },
            getChiTietDoGiatFromApi(searchParamsChiTietDoGiat: ChiTietDoGiatApiSearchParams): void {
                this.loadingTable = true;
                ChiTietDoGiatApi.search(searchParamsChiTietDoGiat).then(res => {
                    this.dsChiTietDoGiat = res.Data;
                    this.searchParamsChiTietDoGiat.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(chiTietDoGiat: ChiTietDoGiat): void {
                this.selectedChiTietDoGiat = chiTietDoGiat;
                this.dialogConfirmDelete = true;
            },
            deleteChiTietDoGiat(): void {
                ChiTietDoGiatApi.delete(this.selectedChiTietDoGiat.ChiTietDoGiatID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getChiTietDoGiatFromApi(this.searchParamsChiTietDoGiat);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
            hide() {
                this.isShow = false
            },
            getDanhSachHinhThucGiat(): void {
                HinhThucGiatApi.search(this.searchParamsHinhThucGiat).then(res => {
                    this.dsHinhThucGiat = res.Data;
                });
            },
            getDanhSachDoGiat(): void {
                DoGiatApi.search(this.searchParamsDoGiat).then(res => {
                    this.dsDoGiat = res.Data;
                });
            },

            getDataFromApi(id: number): void {
                KhachDatHangApi.detail(id).then(res => {
                    this.khachDatHang = res;
                    this.searchParamsChiTietDoGiat.khachDatHangID = this.khachDatHang.KhachDatHangID;
                    this.getChiTietDoGiatFromApi(this.searchParamsChiTietDoGiat);
                    if (this.khachDatHang.TinhTrangThanhToan == 1)
                        this.checkReadOnly = true;
                });
            },
            reloadChiTietDoGiat() {
                this.isUpdateChTiet = false;
                this.$validator.reset();
                this.chiTietDoGiat = {} as ChiTietDoGiat;
            },
            selectedRow(value: any) {
                this.chiTietDoGiat = value;
                this.isUpdateChTiet = true;
            },
            save(): void {
                this.$validator.validateAll('formKhachDatHang').then((res) => {
                    if (res) {
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            KhachDatHangApi.update(this.khachDatHang.KhachDatHangID, this.khachDatHang).then(res => {
                                this.loading = false;
                                this.isShow = false;
                                this.loadingSave = false;
                                this.$emit("save");
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            this.loadingSave = true;
                            KhachDatHangApi.insert(this.khachDatHang).then(res => {
                                this.khachDatHang = res;
                                this.searchParamsChiTietDoGiat.khachDatHangID = res.KhachDatHangID
                                this.isUpdate = true;
                                this.loadingSave = false;
                                this.loading = false;
                                this.$emit("save");
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },
            saveChiTietDoGiat(): void {
                this.chiTietDoGiat.KhachDatHangID = this.khachDatHang.KhachDatHangID;
                this.$validator.validateAll('formChiTietDoGiat').then((res) => {
                    if (res) {
                        this.chiTietDoGiat.DoGiat = undefined;
                        this.chiTietDoGiat.HinhThucGiat = undefined;
                        if (this.isUpdateChTiet) {
                            this.loading = true;
                            this.loadingSave = true;
                            ChiTietDoGiatApi.update(this.chiTietDoGiat.ChiTietDoGiatID, this.chiTietDoGiat).then(res => {
                                this.loading = false;
                                this.isUpdateChTiet = false;
                                this.loadingSave = false;
                                this.$emit("save");
                                this.chiTietDoGiat = {} as ChiTietDoGiat;
                                this.getDonDatHang();
                                this.getChiTietDoGiatFromApi(this.searchParamsChiTietDoGiat);
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = false;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            this.loadingSave = true;
                            ChiTietDoGiatApi.insert(this.chiTietDoGiat).then(res => {
                                this.chiTietDoGiat = res;
                                this.isUpdateChTiet= false;
                                this.loadingSave = false;
                                this.loading = false;
                                this.$emit("save");
                                this.chiTietDoGiat = {} as ChiTietDoGiat;
                                this.getDonDatHang();
                                this.getChiTietDoGiatFromApi(this.searchParamsChiTietDoGiat);
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = false;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },
        }
    });
</script>

