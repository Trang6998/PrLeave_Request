<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Đơn giá giặt</h2>
                <v-layout row wrap>
                    <v-flex xs12 md3>
                        <v-autocomplete v-model="searchParamsDonGia.hinhThucGiatID"
                                        placeholder="Hình thức giặt"
                                        label="Hình thức giặt"
                                        persistent-hint hide-details
                                        @change="getDataFromApi(searchParamsDonGia)"
                                        :items="dsHinhThucGiat" style="margin-top: 0px!important"
                                        item-text="TenHinhThuc"
                                        item-value="HinhThucGiatID" class="mb-2"
                                        clearable></v-autocomplete>
                    </v-flex>
                    <v-flex xs12 md3>
                        <v-autocomplete v-model="searchParamsDonGia.doGiatID"
                                        placeholder="Đồ giặt"
                                        label="Đồ giặt"
                                        persistent-hint hide-details
                                        @change="getDataFromApi(searchParamsDonGia)"
                                        :items="dsDoGiat" style="margin-top: 0px!important"
                                        item-text="TenDoGiat"
                                        item-value="DoGiatID" class="mb-2"
                                        clearable></v-autocomplete>
                    </v-flex>
                    <v-flex xs6 md2>
                        <v-text-field v-model="searchParamsDonGia.giaTu"
                                      @input="getDataFromApi(searchParamsDonGia)"
                                      hide-details
                                      append-icon="search"
                                      label="Giá từ"
                                      placeholder="10.000"
                                      class="mb-2 mt-0"></v-text-field>
                    </v-flex>
                    <v-flex xs6 md2>
                        <v-text-field v-model="searchParamsDonGia.giaDen"
                                      @input="getDataFromApi(searchParamsDonGia)"
                                      hide-details
                                      append-icon="search"
                                      label="Giá đến"
                                      placeholder="50.000"
                                      class="mb-2 mt-0"></v-text-field>
                    </v-flex>
                    <v-flex xs12 md2>
                        <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="float: right " dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                    </v-flex>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsDonGia"
                                      :loading="loadingTable"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsDonGia"
                                      :total-items="searchParamsDonGia.totalItems"                                                
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-xs-center">{{props.index + 1}}</td>
                                <td class="text-xs-center">{{ props.item.HinhThucGiat.TenHinhThuc }}</td>
                                <td class="text-xs-center">{{ props.item.DoGiat.TenDoGiat }}</td>
                                <td class="text-xs-center">{{ props.item.DonGiaGiat }}</td>
                                <td>{{ props.item.GhiChu }}</td>
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
                    <v-btn color="red darken-1" @click.native="deleteDonGia" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-don-gia ref="themSuaDonGia" @save='getDataFromApi(searchParamsDonGia)'></them-sua-don-gia>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import DonGiaApi, { DonGiaApiSearchParams } from '@/apiResources/DonGiaApi';
    import { DonGia } from '@/models/DonGia';
    import HinhThucGiatApi, { HinhThucGiatApiSearchParams } from '@/apiResources/HinhThucGiatApi';
    import { HinhThucGiat } from '@/models/HinhThucGiat';
    import DoGiatApi, { DoGiatApiSearchParams } from '@/apiResources/DoGiatApi';
    import { DoGiat } from '@/models/DoGiat';
    import ThemSuaDonGia from './ThemSuaDonGia.vue';

    export default Vue.extend({
        components: {
            ThemSuaDonGia
        },
        data() {
            return {
                dsHinhThucGiat: [] as HinhThucGiat[],
                dsDoGiat: [] as DoGiat[],
                dsDonGia: [] as DonGia[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: false },
                    { text: 'Tên hình thức giặt', value: 'HinhThucGiat.TenHinhThuc', align: 'center', sortable: true },
                    { text: 'Tên đồ giặt', value: 'DoGiat.TenDoGiat', align: 'center', sortable: true },
                    { text: 'Đơn giá', value: 'DonGiaGiat', align: 'center', sortable: false },
                    { text: 'Ghi chú', value: 'GhiChu', align: 'center', sortable: false },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsDonGia: { includeEntities: true, rowsPerPage: 10 } as DonGiaApiSearchParams,
                searchParamsHinhThucGiat: { includeEntities: true, rowsPerPage: 0 } as HinhThucGiatApiSearchParams,
                searchParamsDoGiat: { includeEntities: true, rowsPerPage: 0 } as DoGiatApiSearchParams,
                loadingTable: false,
                selectedDonGia: {} as DonGia,
                dialogConfirmDelete: false,
                search: "",
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsDonGia);
            this.getDSHinhThucGiat();
            this.getDSDoGiat();
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaDonGia as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsDonGia: DonGiaApiSearchParams): void {
                this.loadingTable = true;
                DonGiaApi.search(searchParamsDonGia).then(res => {
                    this.dsDonGia = res.Data;
                    this.searchParamsDonGia.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            getDSHinhThucGiat() {
                HinhThucGiatApi.search(this.searchParamsHinhThucGiat).then(res => {
                    this.dsHinhThucGiat = res.Data;
                })
            },
            getDSDoGiat() {
                DoGiatApi.search(this.searchParamsDoGiat).then(res => {
                    this.dsDoGiat = res.Data;
                })
            },
            confirmDelete(donGia: DonGia): void {
                this.selectedDonGia = donGia;
                this.dialogConfirmDelete = true;
            },
            deleteDonGia(): void {
                DonGiaApi.delete(this.selectedDonGia.DonGiaID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsDonGia);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

