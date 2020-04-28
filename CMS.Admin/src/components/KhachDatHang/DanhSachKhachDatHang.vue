<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <v-layout nowrap>
                    <h2>Đơn đặt hàng</h2>
                    <v-spacer></v-spacer>
                    <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="float: right " dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12 md6>
                        <v-text-field v-model="searchParamsKhachDatHang.keywords"
                                      @input="getDataFromApi(searchParamsKhachDatHang)"
                                      hide-details
                                      append-icon="search"
                                      label="Tìm kiếm"
                                      placeholder="Tìm kiếm theo tên..."
                                      class="mb-2 mt-1"></v-text-field>
                    </v-flex>
                    <v-flex xs6 md3>
                        <v-datepicker @input="getDataFromApi(searchParamsKhachDatHang)"
                                      v-model="searchParamsKhachDatHang.tuNgay"
                                      label="Từ ngày" hide-details clearable
                                      placeholder="Từ ngày"
                                      type="date"></v-datepicker>
                    </v-flex>
                    <v-flex xs6 md3>
                        <v-datepicker @input="getDataFromApi(searchParamsKhachDatHang)"
                                      v-model="searchParamsKhachDatHang.denNgay"
                                      label="Đến ngày" hide-details clearable
                                      placeholder="Đến ngày"
                                      type="date"></v-datepicker>
                    </v-flex>
                    <v-flex xs12 md4>
                        <label>Trạng thái xử lý</label>
                        <v-radio-group v-model="searchParamsKhachDatHang.trangThai" row hide-details style="margin-top: 0px" @change="getDataFromApi(searchParamsKhachDatHang)">
                            <v-radio label="Chưa xử lý" :value="1"></v-radio>
                            <v-radio label="Đang xử lý" :value="2"></v-radio>
                            <v-radio label="Đã xử lý" :value="3"></v-radio>
                        </v-radio-group>
                    </v-flex>
                    <v-flex xs12 md4>
                        <label>Khung giờ lấy đồ</label>
                        <v-radio-group v-model="searchParamsKhachDatHang.gioLayDo" row hide-details style="margin-top: 0px" @change="getDataFromApi(searchParamsKhachDatHang)">
                            <v-radio label="7h30 - 9h" :value="1"></v-radio>
                            <v-radio label="18h - 20h30" :value="2"></v-radio>
                        </v-radio-group>
                    </v-flex>
                    <v-flex xs12 md4>
                        <label>Trạng thái thanh toán</label>
                        <v-radio-group v-model="searchParamsKhachDatHang.thanhToan" row hide-details style="margin-top: 0px" @change="getDataFromApi(searchParamsKhachDatHang)">
                            <v-radio label="Đã thanh toán" :value="1"></v-radio>
                            <v-radio label="Chưa thanh toán" :value="2"></v-radio>
                        </v-radio-group>
                    </v-flex>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsKhachDatHang"
                                      :loading="loadingTable"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsKhachDatHang"
                                      :total-items="searchParamsKhachDatHang.totalItems"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">

                                <td class="text-md-center" style="width:20px">{{props.index + 1}}</td>
                                <td class="text-md-center">{{ props.item.NguoiDung.TenNguoiDung }}</td>
                                <td class="text-md-center">{{ props.item.NgayDat === null ? "" : props.item.NgayDat|moment('DD/MM/YYYY') }}</td>
                                <td class="text-md-center">{{ props.item.GioDat === 1 ? "7h30 - 9h" : "18h - 20h30" }}</td>
                                <td class="text-md-center">{{ props.item.ThanhTien }}</td>
                                <td class="text-md-center">{{ props.item.TinhTrangXuLy === 1 ? "Chưa xử lý" : props.item.TinhTrangXuLy === 2 ? "Đang xử lý" : "Đã xử lý"  }}</td>
                                <td class="text-md-center">{{ props.item.TinhTrangThanhToan === 1 ? "Đã thanh toán" : "Chưa thanh toán" }}</td>
                                <td class="text-xs-center" style="width:80px;">
                                    <v-layout nowrap>
                                        <v-btn icon small class="mx-0" @click="showModalThemSua(true,props.item)">
                                            <v-icon small color="teal">edit</v-icon>
                                        </v-btn>
                                        <v-btn icon small class="mx-0" @click="confirmDelete(props.item)">
                                            <v-icon small color="pink">delete</v-icon>
                                        </v-btn>
                                    </v-layout>
                                </td>
                            </template>
                        </v-data-table>
                    </v-flex>
                </v-layout>
            </v-card-text>
        </v-card>
        <v-dialog v-model="dialogConfirmDelete" max-width="290">
            <v-card>
                <v-card-title class="headline">Xác nhận xóa</v-card-title>
                <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                    <v-btn color="red darken-1" @click.native="deleteKhachDatHang" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-khach-dat-hang ref="themSuaKhachDatHang" @save='getDataFromApi(searchParamsKhachDatHang)'></them-sua-khach-dat-hang>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import KhachDatHangApi, { KhachDatHangApiSearchParams } from '@/apiResources/KhachDatHangApi';
    import { KhachDatHang } from '@/models/KhachDatHang';
    import ThemSuaKhachDatHang from './ThemSuaKhachDatHang.vue';

    export default Vue.extend({
        components: {
            ThemSuaKhachDatHang
        },
        data() {
            return {
                dsKhachDatHang: [] as KhachDatHang[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: true },
                    { text: 'Tên người dùng', value: 'NguoiDung.TenNguoiDung', align: 'center', sortable: false },
                    { text: 'Ngày đặt', value: 'NgayDat', align: 'center', sortable: true },
                    { text: 'Giờ đặt', value: 'GioDat', align: 'center', sortable: true },
                    { text: 'Thành tiền', value: 'ThanhTien', align: 'center', sortable: false },
                    { text: 'Tình trạng', value: 'TinhTrangXuLy', align: 'center', sortable: true },
                    { text: 'Thanh toán', value: 'TinhTrangThanhToan', align: 'center', sortable: true },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsKhachDatHang: { includeEntities: true, rowsPerPage: 0 } as KhachDatHangApiSearchParams,
                loadingTable: false,
                selectedKhachDatHang: {} as KhachDatHang,
                dialogConfirmDelete: false,
                search: "",
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsKhachDatHang);
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaKhachDatHang as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsKhachDatHang: KhachDatHangApiSearchParams): void {
                this.loadingTable = true;
                KhachDatHangApi.search(searchParamsKhachDatHang).then(res => {
                    this.dsKhachDatHang = res.Data;
                    this.searchParamsKhachDatHang.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(khachDatHang: KhachDatHang): void {
                this.selectedKhachDatHang = khachDatHang;
                this.dialogConfirmDelete = true;
            },
            deleteKhachDatHang(): void {
                KhachDatHangApi.delete(this.selectedKhachDatHang.KhachDatHangID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsKhachDatHang);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

