<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Quản lý đồ giặt</h2>
                <v-layout row wrap>
                    <v-flex xs12 md4>
                        <v-text-field v-model="searchParamsDoGiat.keywords"
                                      @input="getDataFromApi(searchParamsDoGiat)"
                                      hide-details
                                      append-icon="search"
                                      label="Tìm kiếm"
                                      placeholder="Tìm kiếm theo tên..."
                                      class="mb-2 mt-0"></v-text-field>
                    </v-flex>
                    <v-flex xs12 md4>
                        <v-autocomplete v-model="searchParamsDoGiat.loaiDoGiatID"
                                        placeholder="Chọn loại đồ giặt"
                                        label="Loại đồ giặt"
                                        persistent-hint hide-details
                                        @change="getDataFromApi(searchParamsDoGiat)"
                                        :items="dsLoaiDoGiat" style="margin-top: 0px!important"
                                        item-text="TenLoaiDoGiat"
                                        item-value="LoaiDoGiatID" class="mb-2"
                                        clearable></v-autocomplete>
                    </v-flex>

                    <v-flex xs12 md4>
                        <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="float: right " dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                    </v-flex>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsDoGiat"
                                      :loading="loadingTable"
                                      :search="search"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-md-center" style="width:20px">{{props.index + 1}}</td>
                                <td class="text-md-center" style="width:300px">{{ props.item.LoaiDoGiat.TenLoaiDoGiat }}</td>
                                <td class="text-md-center" style="width:300px">{{ props.item.TenDoGiat }}</td>
                                <td class="text-md-center" style="width:300px">{{ props.item.PCS }}</td>
                                <td class="text-md-center" style="width:80px;">
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
                    <v-btn color="red darken-1" @click.native="deleteDoGiat" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-do-giat ref="themSuaDoGiat" @save='getDataFromApi(searchParamsDoGiat)'></them-sua-do-giat>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import DoGiatApi, { DoGiatApiSearchParams } from '@/apiResources/DoGiatApi';
    import { DoGiat } from '@/models/DoGiat';
     import LoaiDoGiatApi, { LoaiDoGiatApiSearchParams } from '@/apiResources/LoaiDoGiatApi';
    import { LoaiDoGiat } from '@/models/LoaiDoGiat';
    import ThemSuaDoGiat from './ThemSuaDoGiat.vue';

    export default Vue.extend({
        components: {
            ThemSuaDoGiat
        },
        data() {
            return {
                dsLoaiDoGiat: [] as LoaiDoGiat[],
                dsDoGiat: [] as DoGiat[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: false },
                    { text: 'Loại đồ giặt', value: 'LoaiDoGiat.TenLoaiDoGiat', align: 'center', sortable: true },
                    { text: 'Tên đồ giặt', value: 'TenDoGiat', align: 'center', sortable: true },
                    { text: 'PCS', value: 'PCS', align: 'center', sortable: false },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsDoGiat: { includeEntities: true, rowsPerPage: 10 } as DoGiatApiSearchParams,
                searchParamsLoaiDoGiat: { includeEntities: true, rowsPerPage: 0 } as LoaiDoGiatApiSearchParams,
                loadingTable: false,
                selectedDoGiat: {} as DoGiat,
                dialogConfirmDelete: false,
                search: "",
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsDoGiat);
            this.getDSLoaiDoGiat()
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaDoGiat as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsDoGiat: DoGiatApiSearchParams): void {
                this.loadingTable = true;
                DoGiatApi.search(searchParamsDoGiat).then(res => {
                    this.dsDoGiat = res.Data;
                    this.searchParamsDoGiat.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            getDSLoaiDoGiat() {
                LoaiDoGiatApi.search(this.searchParamsLoaiDoGiat).then(res => {
                    this.dsLoaiDoGiat = res.Data;
                })
            },
            confirmDelete(doGiat: DoGiat): void {
                this.selectedDoGiat = doGiat;
                this.dialogConfirmDelete = true;
            },
            deleteDoGiat(): void {
                DoGiatApi.delete(this.selectedDoGiat.DoGiatID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsDoGiat);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

