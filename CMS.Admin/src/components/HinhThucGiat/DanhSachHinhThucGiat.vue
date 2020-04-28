<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Hình thức giặt</h2>
                <v-layout row nowrap>
                    <v-text-field v-model="searchParamsHinhThucGiat.keywords" 
                                  @input="getDataFromApi(searchParamsHinhThucGiat)"
                                  hide-details
                                  append-icon="search"
                                  label="Tìm kiếm"
                                  placeholder="Tìm kiếm theo tên..."
                                  class="mb-2 ml-1"></v-text-field>
                    <v-spacer></v-spacer>
                    <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="margin-top: 15px" dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsHinhThucGiat"
                                      :loading="loadingTable" 
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsHinhThucGiat"
                                      :total-items="searchParamsHinhThucGiat.totalItems"                                      
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-xs-center" style="width:15px">{{props.index + 1}}</td>
                                <td class="text-xs-center">{{ props.item.TenHinhThuc }}</td>
                                <td class="text-xs-center">{{ props.item.GhiChu }}</td>
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
                    <v-btn color="red darken-1" @click.native="deleteHinhThucGiat" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-hinh-thuc-giat ref="themSuaHinhThucGiat" @save='getDataFromApi(searchParamsHinhThucGiat)'></them-sua-hinh-thuc-giat>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import HinhThucGiatApi, { HinhThucGiatApiSearchParams } from '@/apiResources/HinhThucGiatApi';
    import { HinhThucGiat } from '@/models/HinhThucGiat';
    import ThemSuaHinhThucGiat from './ThemSuaHinhThucGiat.vue';

    export default Vue.extend({
        components: {
            ThemSuaHinhThucGiat
        },
        data() {
            return {
                search: "",
                dsHinhThucGiat: [] as HinhThucGiat[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: false },
                    { text: 'Tên hình thức giặt', value: 'TenHinhThuc', align: 'center', sortable: true },
                    { text: 'Ghi chú', value: 'GhiChu', align: 'center', sortable: false },
                    { text: 'Thao tác', value: 'ThaoTac', align: 'center', sortable: false },
                ],
                searchParamsHinhThucGiat: { includeEntities: true, rowsPerPage: 0 } as HinhThucGiatApiSearchParams,
                loadingTable: false,
                selectedHinhThucGiat: {} as HinhThucGiat,
                dialogConfirmDelete: false,
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsHinhThucGiat);
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaHinhThucGiat as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsHinhThucGiat: HinhThucGiatApiSearchParams): void {
                this.loadingTable = true;
                HinhThucGiatApi.search(searchParamsHinhThucGiat).then(res => {
                    this.dsHinhThucGiat = res.Data;
                    this.searchParamsHinhThucGiat.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(hinhThucGiat: HinhThucGiat): void {
                this.selectedHinhThucGiat = hinhThucGiat;
                this.dialogConfirmDelete = true;
            },
            deleteHinhThucGiat(): void {
                HinhThucGiatApi.delete(this.selectedHinhThucGiat.HinhThucGiatID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsHinhThucGiat);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

