<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Loại đồ giặt</h2>
                <v-layout row nowrap>
                    <v-text-field v-model="searchParamsLoaiDoGiat.keywords"
                                  @input="getDataFromApi(searchParamsLoaiDoGiat)"
                                  hide-details
                                  append-icon="search"
                                  label="Tìm kiếm"
                                  placeholder="Tìm kiếm theo tên..."
                                  single-line
                                  class="mb-2 ml-1 mt-0 pt-0"></v-text-field>
                    <v-spacer></v-spacer>
                    <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsLoaiDoGiat"
                                      :loading="loadingTable"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsLoaiDoGiat"
                                      :total-items="searchParamsLoaiDoGiat.totalItems"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-xs-center" style="width:20px">{{props.index + 1}}</td>
                                <td class="text-xs-center">{{ props.item.TenLoaiDoGiat }}</td>
                                <td style="width:50%">{{ props.item.MoTa }}</td>
                                <td class="text-xs-center" style="width:50px">
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
                    <v-btn color="red darken-1" @click.native="deleteLoaiDoGiat" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-loai-do-giat ref="themSuaLoaiDoGiat" @save='getDataFromApi(searchParamsLoaiDoGiat)'></them-sua-loai-do-giat>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import LoaiDoGiatApi, { LoaiDoGiatApiSearchParams } from '@/apiResources/LoaiDoGiatApi';
    import { LoaiDoGiat } from '@/models/LoaiDoGiat';
    import ThemSuaLoaiDoGiat from './ThemSuaLoaiDoGiat.vue';

    export default Vue.extend({
        components: {
            ThemSuaLoaiDoGiat
        },
        data() {
            return {
                search: "",
                dsLoaiDoGiat: [] as LoaiDoGiat[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: false },
                    { text: 'Tên loại đồ giặt', value: 'TenLoaiDoGiat', align: 'center', sortable: true },
                    { text: 'Mô tả', value: 'MoTa', align: 'center', sortable: false },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsLoaiDoGiat: { includeEntities: true, rowsPerPage: 10 } as LoaiDoGiatApiSearchParams,
                loadingTable: false,
                selectedLoaiDoGiat: {} as LoaiDoGiat,
                dialogConfirmDelete: false,
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsLoaiDoGiat);
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaLoaiDoGiat as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsLoaiDoGiat: LoaiDoGiatApiSearchParams): void {
                this.loadingTable = true;
                LoaiDoGiatApi.search(searchParamsLoaiDoGiat).then(res => {
                    this.dsLoaiDoGiat = res.Data;
                    this.searchParamsLoaiDoGiat.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(loaiDoGiat: LoaiDoGiat): void {
                this.selectedLoaiDoGiat = loaiDoGiat;
                this.dialogConfirmDelete = true;
            },
            deleteLoaiDoGiat(): void {
                LoaiDoGiatApi.delete(this.selectedLoaiDoGiat.LoaiDoGiatID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsLoaiDoGiat);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

