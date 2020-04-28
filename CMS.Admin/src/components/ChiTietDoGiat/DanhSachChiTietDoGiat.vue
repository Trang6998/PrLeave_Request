<template>
    <v-flex xs12>
        <v-breadcrumbs divider="/" class="pa-0">
            <v-icon slot="divider">chevron_right</v-icon>
            <v-breadcrumbs-item>
                <v-btn flat class="ma-0" @click="$router.go(-1)" small><v-icon>arrow_back</v-icon> Quay lại</v-btn>
            </v-breadcrumbs-item>
            <v-breadcrumbs-item to="/chitietdogiat" exact>ChiTietDoGiat</v-breadcrumbs-item>
        </v-breadcrumbs>
        <v-card>
            <v-card-text>
                <v-layout row wrap>
                    <v-flex xs12>
                    <v-data-table :headers="tableHeader"
                                :items="dsChiTietDoGiat"
                                @update:pagination="getDataFromApi" :pagination.sync="searchParamsChiTietDoGiat"
                                :total-items="searchParamsChiTietDoGiat.totalItems"
                                :loading="loadingTable"
                                class="table-border table">
                        <template slot="items" slot-scope="props">
                    <td>{{ props.item.ChiTietDoGiatID }}</td>
                            <td>{{ props.item.KhachDatHang.KhachDatHangID }}</td>
                            <td>{{ props.item.DoGiat.DoGiatID }}</td>
                    <td>{{ props.item.HinhThucGiatID }}</td>
                    <td>{{ props.item.SoLuong }}</td>
                    <td class="text-xs-center" style="width:80px;">
                        <v-btn flat icon small :to="'/chitietdogiat/'+props.item.ChiTietDoGiatID" class="ma-0">
                            <v-icon small>edit</v-icon>
                        </v-btn>
                        <v-btn flat color="red" icon small class="ma-0" @click="confirmDelete(props.item)">
                            <v-icon small>delete</v-icon>
                        </v-btn>
                    </td>
                            </template>
                        </v-data-table>
                    </v-flex xs12>
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
                    <v-btn color="red darken-1" @click.native="deleteChiTietDoGiat" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import ChiTietDoGiatApi, { ChiTietDoGiatApiSearchParams } from '@/apiResources/ChiTietDoGiatApi';
    import { ChiTietDoGiat } from '@/models/ChiTietDoGiat';

    export default Vue.extend({
        components: {},
        data() {
            return {
                dsChiTietDoGiat: [] as ChiTietDoGiat[],
                tableHeader: [
                    { text: 'ChiTietDoGiatID', value: 'ChiTietDoGiatID', align: 'center', sortable: true },
                    { text: 'KhachDatHangID', value: 'KhachDatHang.KhachDatHangID', align: 'center', sortable: true },
                    { text: 'DoGiatID', value: 'DoGiat.DoGiatID', align: 'center', sortable: true },
                    { text: 'HinhThucGiatID', value: 'HinhThucGiatID', align: 'center', sortable: true },
                    { text: 'SoLuong', value: 'SoLuong', align: 'center', sortable: true },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsChiTietDoGiat: { includeEntities: true, rowsPerPage: 10 } as ChiTietDoGiatApiSearchParams,
                loadingTable: false,
                selectedChiTietDoGiat: {} as ChiTietDoGiat,
                dialogConfirmDelete: false,
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsChiTietDoGiat);
        },
        methods: {
            getDataFromApi(searchParamsChiTietDoGiat: ChiTietDoGiatApiSearchParams): void {
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
                    this.getDataFromApi(this.searchParamsChiTietDoGiat);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

